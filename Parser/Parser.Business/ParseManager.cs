﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

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
    }

    public class ParseManager
    {
        private const string refer = "http://abit-poisk.org.ua/rate2014/univer/";

        private const string mainRefer = "http://abit-poisk.org.ua/score/direction/";

        private const string bachelorPattern = @"<tr>(?:[\s\w\/\\]*?)?<td(?:[а-яА-ЯіїєІЇЄ=\s\w""\\-]*?)?>(?:[\s\w.\/\\]*?)?(?:Бакалавр|Спеціаліст на основі повної загальної середньої освіти )(?:[\s\w.\/\\]*?)?</td>(?:[:;""',.а-яА-ЯіїєІЇЄ=?()\s\w\/\\<>-]*?)</tr>";

        private const string linkPattern = @"href=(?:[\w\\])?""/score/direction/([\d]*)";

        private const string universityPattern = @"<dt>ВНЗ:</dt>(?:[\s\w\/\\<]*?)><a(?:[\s\w=""\\\/]*)>([-\w\sа-яА-ЯіІєЄїЇ\\\/""',.()]*)?</a>";

        private const string facultyPattern = @"<dt>Факультет:</dt>(?:[\s\w\/\\<]*?)>([-\w\sа-яА-ЯіІєЄїЇ\\\/""',.()]*)?";

        private const string specialityPattern = @"<dt>(?:Напрям:|Спеціальність:)</dt>(?:[\s\w\/\\<]*?)>([-\w\sа-яА-ЯіІєЄїЇ\\\/""',.()]*)?";
        
        private const int startPageId = 1;

        private const int endPageId = 1607;

        private List<UniversityData> universities;

        private UniversityData currentUniversity;

        private bool IsUniversity;

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
                    //else if (currentUniversity.UniversityName == null && IsUniversity)
                    //{
                    //    id--;
                    //}
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
                    //WebOperationContext ctx = WebOperationContext.Current;
                    //if (ctx.OutgoingResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    //{
                    //    SaveToBinary();
                    //}
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

            Regex reg = new Regex(bachelorPattern);
            foreach (Match match in reg.Matches(text))
            {
                string tmp = match.Groups[0].Value.ToString();
                Regex innerReg = new Regex(linkPattern);
                Match found = innerReg.Match(tmp);
                ParseSpecialityDetails(mainRefer + found.Groups[1].Value.ToString());
            }
        }

        private void ParseSpecialityDetails(string pageRefer)
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
                    currentUniversity.RegionName = TakeRegion(universityName);
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

        private string TakeRegion(string universityName)
        {
            string region = "";

            if (universityName.Contains("Київський")) region = "Київ";
            else if (universityName.Contains("Севастополь")) region = "Севастополь";
            else if (universityName.Contains("АР Крим")) region = "АР Крим";
            else if (universityName.Contains("Вінницький")) region = "Вінницька область";
            else if (universityName.Contains("Луцький")) region = "Волинська область";
            else if (universityName.Contains("Дніпропетровський")) region = "Дніпропетровська область";
            else if (universityName.Contains("Донецький")) region = "Донецька область";
            else if (universityName.Contains("Житомирський")) region = "Житомирська область";
            else if (universityName.Contains("Ужгородський")) region = "Закарпатська область";
            else if (universityName.Contains("Запорізький")) region = "Запорізька область";
            else if (universityName.Contains("Івано-Франківський")) region = "Івано-Франківська область";
            else if (universityName.Contains("Кіровоградський")) region = "Кіровоградська область";
            else if (universityName.Contains("Луганський")) region = "Луганська область";
            else if (universityName.Contains("Львівський")) region = "Львівська область";
            else if (universityName.Contains("Миколаївський")) region = "Миколаївська область";
            else if (universityName.Contains("Одеський")) region = "Одеська область";
            else if (universityName.Contains("Полтавський")) region = "Полтавська область";
            else if (universityName.Contains("Рівненський")) region = "Рівненська область";
            else if (universityName.Contains("Сумський")) region = "Сумська область";
            else if (universityName.Contains("Тернопільський")) region = "Тернопільска область";
            else if (universityName.Contains("Харківський")) region = "Харківська область";
            else if (universityName.Contains("Херсонський")) region = "Херсонська область";
            else if (universityName.Contains("Хмельницький")) region = "Хмельницка область";
            else if (universityName.Contains("Черкаський")) region = "Черкаська область";
            else if (universityName.Contains("Чернівецький")) region = "Чернівецька область";
            else if (universityName.Contains("Чернігівський")) region = "Чернігівська область";

            return region;
        }

        private void SaveToXml()
        {
            
        }

        private void SaveToBinary()
        {
            using (Stream stream = File.Open("Universities.dll", FileMode.Create))
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
                using (Stream stream = File.Open("Universities.dll", FileMode.Open))
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
            using (Stream stream = File.Open("Universities.dll", FileMode.Open))
            {
                var bformatter = new BinaryFormatter();

                Console.WriteLine("Reading Universities Information");
                universities = (List<UniversityData>)bformatter.Deserialize(stream);
            }
        }
    }
}
