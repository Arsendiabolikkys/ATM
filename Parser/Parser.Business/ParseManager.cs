using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser.Business
{
    public class FacultyData
    {
        public string FacultyName;

        public List<string> Specialities;

        public FacultyData()
        {
            Specialities = new List<string>();
        }
    }

    public class UniversityData
    {
        public string UniversityName;

        public string RegionName;

        public List<FacultyData> Faculties;

        public UniversityData()
        {
            Faculties = new List<FacultyData>();
        }
    }

    public class ParseManager
    {
        private const string refer = "http://abit-poisk.org.ua/rate2014/univer/";

        private const string mainRefer = "http://abit-poisk.org.ua/score/direction/";

        private const string bachelorPattern = @"<tr>(?:[\s\w\/\\]*?)?<td(?:[а-яА-ЯіїєІЇЄ=\s\w""\\-]*?)?>(?:[\s\w.\/\\]*?)?Бакалавр(?:[\s\w.\/\\]*?)?</td>(?:[:;""',.а-яА-ЯіїєІЇЄ=?()\s\w\/\\<>-]*?)</tr>";

        private const string linkPattern = @"href=(?:[\w\\])?""/score/direction/([\d]*)";

        private const string universityPattern = @"<dt>ВНЗ:</dt>(?:[\s\w\/\\<]*?)><a(?:[\s\w=""\\\/]*)>([\w\sа-яА-ЯіІєЄїЇ\\\/""]*)?</a>";

        private const string facultyPattern = @"<dt>Факультет:</dt>(?:[\s\w\/\\<]*?)>([\w\sа-яА-ЯіІєЄїЇ\\\/""]*)?";

        private const string specialityPattern = @"<dt>Напрям:</dt>(?:[\s\w\/\\<]*?)>([\w\sа-яА-ЯіІєЄїЇ\\\/""]*)?";
        
        private const int startPageId = 1;

        private const int endPageId = 1607;

        private List<UniversityData> universities;

        private UniversityData currentUniversity;

        public ParseManager()
        {            
            universities = new List<UniversityData>();
            currentUniversity = null;
        }

        public void StartParse()
        {            
            for (var id = startPageId; id <= endPageId; id++)
            {
                ParseUniverPage(refer + id);
                currentUniversity = null;
                Console.WriteLine("Ссылка: " + refer + id);
            }
            SaveToXml();
        }

        private void ParseUniverPage(string pageRefer)
        {
            try
            {
                currentUniversity = new UniversityData();
                string text;

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(pageRefer);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

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
            catch (Exception ex)
            { 
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
                Match universityName = univerRegex.Match(source);
                if (String.IsNullOrEmpty(currentUniversity.UniversityName))
                    currentUniversity.UniversityName = universityName.Value.ToString();
                //Нужно очистить от "колледж", "&quot" и тд
                //По названию города в названии вуза определить область вуза

                Regex facultyRegex = new Regex(facultyPattern);
                Match facultyName = univerRegex.Match(source);
                //если нету такого факультета - добавить

                Regex specRegex = new Regex(specialityPattern);
                Match specName = univerRegex.Match(source);
                //к новому или уже существующему факультету добавить специальность

            }            
            catch(Exception ex)
            {
            }
        }

        private void SaveToXml()
        {
 
        }
    }
}
