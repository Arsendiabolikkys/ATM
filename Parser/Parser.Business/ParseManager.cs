using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Linq;

namespace Parser.Business
{
    [Serializable]
    public class FacultyData
    {
        public string FacultyName;

        public List<string> Specialities;

        public FacultyData()
        {            
            Specialities = new List<string>();
        }

        public FacultyData(string facultyName)
        {
            FacultyName = facultyName;
            Specialities = new List<string>();
        }

        public void AddSpeciality(string specialityName)
        {
            Specialities.Add(specialityName);
        }
    }

    [Serializable]
    public class UniversityData
    {
        public string UniversityName;

        public string RegionName;

        public string CityName;

        public List<FacultyData> Faculties;

        public UniversityData()
        {
            Faculties = new List<FacultyData>();
        }

        public bool IsFacultyExist(string facultyName)
        {
            return Faculties.Any(faculty => faculty.FacultyName == facultyName);
        }

        public void AddSpecialityToFaculty(string facultyName, string specName)
        {
            Faculties.Find(faculty => faculty.FacultyName == facultyName).AddSpeciality(specName);
        }

        public void FillCity(string regionName)
        {
            if (regionName.Contains("Київська")) CityName = "Біла Церква";
            else if (regionName.Contains("Київ")) CityName = "Київ";
            else if (regionName.Contains("Севастополь")) CityName = "Севастополь";
            else if (regionName.Contains("АР Крим")) CityName = "Сімферополь";
            else if (regionName.Contains("Вінницька")) CityName = "Вінниця";
            else if (regionName.Contains("Волинська")) CityName = "Луцьк";
            else if (regionName.Contains("Дніпропетровська")) CityName = "Дніпропетровськ";
            else if (regionName.Contains("Донецька")) CityName = "Донецьк";
            else if (regionName.Contains("Житомирська")) CityName = "Житомир";
            else if (regionName.Contains("Закарпатська")) CityName = "Ужгород";
            else if (regionName.Contains("Запорізька")) CityName = "Запоріжжя";
            else if (regionName.Contains("Івано-Франківська")) CityName = "Івано-Франківськ";
            else if (regionName.Contains("Кіровоградська")) CityName = "Кіровоград";
            else if (regionName.Contains("Луганська")) CityName = "Луганськ";
            else if (regionName.Contains("Львівська")) CityName = "Львів";
            else if (regionName.Contains("Миколаївська")) CityName = "Миколаїв";
            else if (regionName.Contains("Одеська")) CityName = "Одеса";
            else if (regionName.Contains("Полтавська")) CityName = "Полтава";
            else if (regionName.Contains("Рівненська")) CityName = "Рівне";
            else if (regionName.Contains("Сумська")) CityName = "Суми";
            else if (regionName.Contains("Тернопільська")) CityName = "Тернопіль";
            else if (regionName.Contains("Харківська")) CityName = "Харків";
            else if (regionName.Contains("Херсонська")) CityName = "Херсон";
            else if (regionName.Contains("Хмельницька")) CityName = "Хмельницкий";
            else if (regionName.Contains("Черкаська")) CityName = "Черкаси";
            else if (regionName.Contains("Чернівецька")) CityName = "Чернівці";
            else if (regionName.Contains("Чернігівська")) CityName = "Чернігів";
        }
    }

    public class ParseManager
    {
        private const string refer = "http://abit-poisk.org.ua/rate2014/univer/";

        private const string mainRefer = "http://abit-poisk.org.ua/score/direction/";

        private const string fileName = "Universities2.dll";

        private const string bachelorPattern = @"<tr>(?:[\s\w\/\\]*?)?<td(?:[а-яА-ЯіїєІЇЄ=\s\w""\\-]*?)?>(?:[\s\w.\/\\]*?)?(?:Бакалавр|Спеціаліст на основі повної загальної середньої освіти )(?:[\s\w.\/\\]*?)?</td>(?:[:;""',.а-яА-ЯіїєІЇЄ=?()\s\w\/\\<>-]*?)</tr>";

        private const string linkPattern = @"href=(?:[\w\\])?""/score/direction/([\d]*)";

        private const string universityPattern = @"<dt>ВНЗ:</dt>(?:[\s\w\/\\<]*?)><a(?:[\s\w=""\\\/]*)>([-\w\sа-яА-ЯіІєЄїЇ\\\/""',.()]*)?</a>";

        private const string facultyPattern = @"<dt>Факультет:</dt>(?:[\s\w\/\\<]*?)>([-\w\sа-яА-ЯіІєЄїЇ\\\/""',.()]*)?";

        private const string specialityPattern = @"<dt>(?:Напрям:|Спеціальність:)</dt>(?:[\s\w\/\\<]*?)>([-\w\sа-яА-ЯіІєЄїЇ\\\/""',.()]*)?";

        private const string regionPattern = @"<a href=""/rate2014/region/(?:[\d]*)"">([-.\w\sа-яА-ЯіІїЇєЄ']*)";
        
        private const int startPageId = 1;

        private const int endPageId = 1607;

        private List<UniversityData> universities;

        private UniversityData currentUniversity;

        private bool IsUniversity;

        Dictionary<int, string> savedRegs = new Dictionary<int, string>();

        Dictionary<int, string> savedUniversities = new Dictionary<int, string>();

        Dictionary<int, string> savedSpecialities = new Dictionary<int, string>();

        Dictionary<int, string> savedFaculties = new Dictionary<int, string>();

        public ParseManager()
        {
            IsUniversity = true;
            universities = new List<UniversityData>();
            currentUniversity = null;
        }

        public void StartParse()
        {
            if (FileExist())
            {
                ReadFromBinary();
            }
            
            for (var id = universities.Count == 0 ? startPageId : universities.Count + 1; id <= endPageId; id++)
            {
                try 
                {
                    ParseUniverPage(refer + id);
                    
                    if (currentUniversity.UniversityName != null && IsUniversity)
                    {
                        universities.Add(currentUniversity);
                        currentUniversity = null;
                    }
                    if (id % 5 == 0)
                    {
                        Console.WriteLine("id%10==0");
                        SaveToBinary();
                    }
                    Console.WriteLine("Ссылка: " + refer + id);
                    if (!IsUniversity)
                        IsUniversity = true;
                }
                catch (Exception ex)
                {
                }
            }
            //SaveToXml();
            //SaveToBinary();
        }

        private void ParseUniverPage(string pageRefer)
        {
            currentUniversity = new UniversityData();
            string text;
            HttpWebRequest req = null;
            HttpWebResponse resp = null;

            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(pageRefer);
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                while (resp == null)
                {
                    resp = (HttpWebResponse)req.GetResponse();
                }
            }            

            using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                text = stream.ReadToEnd();
            }

            Regex regionRegex = new Regex(regionPattern);
            Match regionMatch = regionRegex.Match(text);
            string regionName = regionMatch.Groups[1].Value.ToString();

            Regex reg = new Regex(bachelorPattern);
            foreach (Match match in reg.Matches(text))
            {
                string tmp = match.Groups[0].Value.ToString();
                Regex innerReg = new Regex(linkPattern);
                Match found = innerReg.Match(tmp);
                ParseSpecialityDetails(mainRefer + found.Groups[1].Value.ToString(), regionName);
            }
        }

        private void ParseSpecialityDetails(string pageRefer, string regionName)
        {
            try
            {       
                string source;

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(pageRefer);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    source = stream.ReadToEnd();
                }

                Regex univerRegex = new Regex(universityPattern);
                Match universityMatch = univerRegex.Match(source);
                string universityName = universityMatch.Groups[1].Value.ToString();

                RemoveQuot(universityName);
                if (IsCollege(universityName))
                    throw new Exception("Коледж");
                //Нужно очистить от "колледж", "&quot" и тд
                //По названию города в названии вуза определить область вуза
                if (String.IsNullOrEmpty(currentUniversity.UniversityName))
                {
                    currentUniversity.UniversityName = universityName;
                    currentUniversity.RegionName = regionName;
                    currentUniversity.FillCity(regionName);
                }

                Regex facultyRegex = new Regex(facultyPattern);
                Match facultyMatch = facultyRegex.Match(source);
                string facultyName = facultyMatch.Groups[1].Value.ToString();
                if (!currentUniversity.IsFacultyExist(facultyName))
                    currentUniversity.Faculties.Add(new FacultyData(facultyName));
                //если нету такого факультета - добавить

                Regex specRegex = new Regex(specialityPattern);
                Match specMatch = specRegex.Match(source);
                string specName = specMatch.Groups[1].Value.ToString();
                currentUniversity.AddSpecialityToFaculty(facultyName, specName);
                //к новому или уже существующему факультету добавить специальность

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsUniversity = false;
            }
        }

        private void RemoveQuot(string tmp)
        {
            tmp.Replace("&quot", "\"");
        }

        private bool IsCollege(string universityName)
        {
            string tmp = universityName.ToLower();
            return tmp.Contains("коледж") || universityName.Contains("технікум");
        }

        public void SaveToXml()
        {
            if (FileExist())
            {
                ReadFromBinary();
                XDocument facultDocument = new XDocument();
                XDocument specDocument = new XDocument();
                XDocument regionDocument = new XDocument();

                SaveRegions();

                SaveUniversities();

                SaveFaculties();

                SaveSpecialities();

                SaveFacultSpec();
            }
        }

        private void SaveRegions()
        {
            int regId = 0;
            XDocument regionDocument = new XDocument();
            var regions = new XElement("regions");

            regionDocument.Add(regions);

            foreach (var univer in this.universities)
            {
                if (!String.IsNullOrEmpty(univer.RegionName) && !savedRegs.ContainsValue(univer.RegionName))
                {
                    savedRegs.Add(regId, univer.RegionName);

                    regions.Add(new XElement("region",
                        new XElement("id", regId++),
                        new XElement("name", univer.RegionName)));
                }
            }

            regionDocument.Save("regions.xml");
        }

        private void SaveUniversities()
        {

            int univerId = 0;

            XDocument univerDocument = new XDocument();

            var universities = new XElement("universities");

            univerDocument.Add(universities);

            foreach(var univer in this.universities)
            {
                savedUniversities.Add(univerId, univer.UniversityName);
                universities.Add(new XElement("university",
                    new XElement("id", univerId++),
                    new XElement("name", univer.UniversityName),
                    new XElement("cityId", GetRegionIdByName(univer.RegionName))));
            }

            univerDocument.Save("universities.xml");
        }

        private void SaveFaculties()
        {
            int facultyId = 0;

            XDocument facultyDocument = new XDocument();

            var faculties = new XElement("faculties");

            facultyDocument.Add(faculties);


            foreach (var univer in this.universities)
            {
                foreach(var facult in univer.Faculties)
                {
                    if(!savedFaculties.ContainsValue(facult.FacultyName))
                    {
                        savedFaculties.Add(facultyId, facult.FacultyName);
                        faculties.Add(new XElement("faculty",
                            new XElement("id", facultyId++)
                            , new XElement("name", facult.FacultyName),
                            new XElement("univerId", GetUniversityIdByName(univer.UniversityName))));
                    }
                }
            }

            facultyDocument.Save("faculties.xml");
        }

        private void SaveSpecialities()
        {
            int specId = 0;

            List<string> specNames = new List<string>();

            XDocument specDocument = new XDocument();

            var specialities = new XElement("specialities");

            specDocument.Add(specialities);

            foreach (var univer in this.universities)
            {
                foreach(var facult in univer.Faculties)
                {
                    foreach(var spec in facult.Specialities)
                    {
                        if(!savedSpecialities.ContainsValue(spec))
                        {
                            savedSpecialities.Add(specId, spec);
                            specialities.Add(new XElement("speciality",
                                new XElement("id", specId++),
                                new XElement("name", spec)));
                        }
                    }
                }
            }

            specDocument.Save("specialities.xml");
        }

        private void SaveFacultSpec()
        {
            XDocument facultSpecDocument = new XDocument();

            var facultSpec = new XElement("facultSpec");

            facultSpecDocument.Add(facultSpec);


            foreach (var univer in this.universities)
            {
                foreach (var facult in univer.Faculties)
                {
                    foreach(var spec in savedSpecialities)
                    {
                        if(facult.Specialities.Contains(spec.Value))
                        {
                            facultSpec.Add(new XElement("pair",
                                new XElement("facultId", GetFacultItByName(facult.FacultyName)),
                                new XElement("specId", spec.Key)));
                        }
                    }
                }
            }

            facultSpecDocument.Save("facultSpec.xml");
        }

        private int GetRegionIdByName(string name)
        {
            return savedRegs.FirstOrDefault(reg => reg.Value == name).Key;
        }

        private int GetUniversityIdByName(string name)
        {
            return savedUniversities.FirstOrDefault(reg => reg.Value == name).Key;
        }

        private int GetFacultItByName(string name)
        {
            return savedFaculties.FirstOrDefault(reg => reg.Value == name).Key;
        }

        private void SaveToBinary()
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                var bformatter = new BinaryFormatter();

                Console.WriteLine("Writing Universities Information");
                bformatter.Serialize(stream, universities);
            }
        }


        private bool FileExist()
        {
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {                    
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ReadFromBinary()
        {
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                var bformatter = new BinaryFormatter();

                Console.WriteLine("Reading Universities Information");
                universities = (List<UniversityData>)bformatter.Deserialize(stream);
            }
        }
    }
}
