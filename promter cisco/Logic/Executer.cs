using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Suggeritore_Cisco.Logic
{
    class Engine
    {
        private const double ACCEPTABLE_MINIMUM_PRECISION_THRESHOLD = 45;

        private List<QuestionAnswer> listQuestionAnswers;

        private string cisco;
        private struct QuestionAnswer
        {
            public string Question;
            public string Answer;
        }

        public Engine(string capitolo)
        {
            listQuestionAnswers = new List<QuestionAnswer>();

            //Read from resource
            using (var st = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Suggeritore_Cisco.Resources.Cisco.xml"))
            {
                var bytes = new byte[st.Length];
                st.Read(bytes, 0, (int)st.Length);
                cisco = Encoding.UTF8.GetString(bytes);
            }

            var doc = new XmlDocument();

            //Parse xml file.
            using (var reader = XmlReader.Create(new StringReader(cisco)))
            {
                doc.Load(reader);
                foreach (XmlNode xn in doc) //Tutti i nodi del documento
                    if (xn.Name == "COLLECTION") //Vede se il nodo corrisponde a "COLLECTION"
                        foreach (XmlNode node in xn.ChildNodes) //Legge i sottonodi del nodo "COLLECTION"
                            if (node.Name == "CISCO" && node.Attributes["capitolo"].Value == capitolo) //Vede se il nodo corrisponde a "CISCO" e che abbia come uguaglianza il capitolo con il parametro
                                foreach (XmlNode nodoRichiesta in node) //Preleva tutti i sottonodi del capitolo scelto
                                    listQuestionAnswers.Add(new QuestionAnswer { Question = nodoRichiesta.ChildNodes[0].InnerText.Replace("\\n","\n"), Answer = nodoRichiesta.ChildNodes[1].InnerText.Replace("\\n", "\n") }); // lo aggiunge

            }

            listQuestionAnswers.Sort((a1, a2) => a2.Question.Length.CompareTo(a1.Question.Length));
        }
        public KeyValuePair<string[], double> GetRisposta(string domanda)
        {
            switch (domanda) //Incomplete request.
            {
                case "Refer to the exhibit.":
                case "Refer to the exhibit":
                case "Fill in the blank.":
                case "Fill in the blank":
                    return new KeyValuePair<string[], double>(new[] { "Incomplete request", "Incomplete request" }, 100);
            }

            var correspondence = new List<KeyValuePair<QuestionAnswer, double>>(listQuestionAnswers.Count);

            Parallel.ForEach(listQuestionAnswers, (dr) => //Multithreaded as comparing char per char is cpu heavy.
            {
                var computedPercentage = ComputePercentage(domanda, dr.Question);
                if (computedPercentage >= ACCEPTABLE_MINIMUM_PRECISION_THRESHOLD) //Add to the list if the correspondence percentage between 2 strings is equal or more to ACCEPTABLE_MINIMUM_PRECISION_THRESHOLD value.
                    correspondence.Add(new KeyValuePair<QuestionAnswer, double>(dr, computedPercentage)); //Then add it.
            });

            correspondence.Sort((a1, a2) => a2.Value.CompareTo(a1.Value)); //order by percent

            if (correspondence.Count == 0) return new KeyValuePair<string[], double>(null, 0); //If 0 means no corresponding question was found.

            var percentage = correspondence[0].Value;
            return new KeyValuePair<string[], double>(new[] {correspondence[0].Key.Question, correspondence[0].Key.Answer}, percentage); //return a new KeyValyePair containing the corresponding question and answer.
        }

        private static double ComputePercentage(string str1, string str2)
        {
            var str2Lng = str2.Length;
            var distance = str1.DamerauLevenshteinDistanceTo(str2);
            var distanceDifference = str2Lng - distance;
            return (str2Lng - (str2Lng - distanceDifference)) / (double)str2Lng * 100;
        }
    }
}