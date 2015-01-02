using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Weather.Domain.Webservices
{
    public class GeonamesWebservice
    {
        private IEnumerable<GeonameLocation> searchGeonames(string searchString){            

            string requestUriString = String.Format("http://api.geonames.org/search?name=" + searchString + "&username=" + "grayfish" );
            //TODO: Användarnamnet SKA ligga i Web.configen! (Finns där under namnet geonamesUser, 
            //men hur hämtar man ut det härifrån? Skicka med som paramter?)

            XmlDocument content = new XmlDocument(); //Där datan ska läggas

            var request = (HttpWebRequest)WebRequest.Create(requestUriString); 

            using(var response = request.GetResponse()){
                content.Load(response.GetResponseStream()); // Läser in XML filen i mitt XML-dokument, då kan jag använda content-variabeln för att läsa av innehållet
            }

            List<GeonameLocation> listToReturn = new List<GeonameLocation>();

            var locations = content.GetElementsByTagName("geoname"); //Här gör jag en array av alla "geoname"'s i xmlfilen. Alltså alla locations... 

            foreach (System.Xml.XmlElement obj in locations)
            {
                GeonameLocation location = new GeonameLocation(
                    obj["name"].InnerText,
                    obj["lng"].InnerText,
                    obj["lat"].InnerText,
                    obj["countryName"].InnerText
                    //"empty" //obs, måste ha läst fel, här skulle region vara, men det finns ingen region... typ östergötland
                    );

                listToReturn.Add(location);
            }

            return listToReturn.AsEnumerable<GeonameLocation>();
           
        }

        public IEnumerable<GeonameLocation> preGeonamesSearch(string searchString)
        { // Här ska cachening göras...


            return searchGeonames(searchString);

        }
    }
}
