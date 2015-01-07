using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Weather.Domain.Webservices
{
    public class YrWebservice
    {
        private IEnumerable<Forecast> GetForecastFromLaNLo(string latitude, string longtitude)
        {

            string requestUriString = String.Format("http://api.yr.no/weatherapi/locationforecast/1.9/?lat="+latitude+";lon="+longtitude);

            XmlDocument content = new XmlDocument(); //Där datan ska läggas

            var request = (HttpWebRequest)WebRequest.Create(requestUriString);

            using (var response = request.GetResponse())
            {
                content.Load(response.GetResponseStream()); // Läser in XML filen i mitt XML-dokument, då kan jag använda content-variabeln för att läsa av innehållet
            }

            List<Forecast> listToReturn = new List<Forecast>();

            var Forecasts = content.GetElementsByTagName("time"); //Här gör jag en array av alla "time" ....



            string temperature = "notSet";
            string pic = "notSet";
            List<string> backupTemp = new List<string>();
            List<string> backupPic = new List<string>();
            var dateCounter = new DateTime().Date;
            bool newDateFlag = false;
            bool first = true;
            foreach (System.Xml.XmlElement obj in Forecasts)
            {
                if(first){ //Första gången ska alltid datumet sättas till datumet som är först i foreach-loopen...
                    first = false;
                    dateCounter = Convert.ToDateTime(obj.GetAttribute("to")).Date;
                }

                if (dateCounter != Convert.ToDateTime(obj.GetAttribute("to")).Date)
                {
                    newDateFlag = true;                    
                    dateCounter = Convert.ToDateTime(obj.GetAttribute("to")).Date;                    
                }
                else { newDateFlag = false; }

                
                if (obj.GetAttribute("from") == obj.GetAttribute("to") && temperature == "notSet")
                {//Om både From och To är samma  
                    if (Convert.ToDateTime(obj.GetAttribute("from")).TimeOfDay == new TimeSpan(15, 0, 0))
                    {//och även klockan 15 på dagen så kan temperaturen hämtas ut
                        temperature = obj["location"]["temperature"].GetAttribute("value");
                    }
                    else
                    {//om vi inte har klockslaget 15 på både To och From så måste vi ha ett alternativ som används om det byts dag innan temp har satts...
                        backupTemp.Add(temperature = obj["location"]["temperature"].GetAttribute("value"));
                    }
                    
                }
                if (Convert.ToDateTime(obj.GetAttribute("from")).TimeOfDay == new TimeSpan(12, 0, 0) && Convert.ToDateTime(obj.GetAttribute("to")).TimeOfDay == new TimeSpan(18, 0, 0) && pic == "notSet")
                {//om from är 12.00 och to är 18.00 så kan bilden hämtas ut (Vilken bild som ska användas...)
                    pic = obj["location"]["symbol"].GetAttribute("number");
                }
                else if (obj.GetAttribute("from") != obj.GetAttribute("to") && (Convert.ToDateTime(obj.GetAttribute("to")).TimeOfDay < new TimeSpan(24, 0, 0) && Convert.ToDateTime(obj.GetAttribute("to")).TimeOfDay > new TimeSpan(12, 0, 0)))
                {// En backup om det inte hittas några på pricken... Denna tar fram tider mellan 12.00 och 18.00...
                    backupPic.Add(obj["location"]["symbol"].GetAttribute("number"));
                }

                if (newDateFlag)
                {

                    if (pic == "notSet")
                    {
                        if (backupPic.Count >= 1)
                        {
                            pic = backupPic[0];
                        }
                        else
                        {// om ingen bild fanns mellan 12.00 och 18.00 så ska föregående dags bild visas...
                            if (listToReturn.Count != 0)
                            {
                                pic = listToReturn[listToReturn.Count - 1].PictureName;
                            }
                            else
                            {
                                pic = "unkown";// om det inte går att få tag på föregående dags bild
                            }                            
                        }
                                                
                    }
                    if (temperature == "notSet")
                    {
                        temperature = backupTemp[0];
                    }

                    Forecast Forecast = new Forecast(
                    Convert.ToDateTime(obj.GetAttribute("from")) - new TimeSpan(1, 0, 0, 0), //Eftersom detta görs vid ny dag så tar vi minus 1 dag...
                    temperature,
                    pic
                    );

                    
                    listToReturn.Add(Forecast);
                    //Förbereder att så att ny dag kan hämtas...
                    temperature = "notSet";
                    pic = "notSet";
                    backupTemp.Clear();
                    backupPic.Clear();

                    if (listToReturn.Count >= 5)
                    {// om vi har 5 föremål i vår lista så har vi för alla dagar vi behöver...
                        break;
                    }
                    
                }
                
                
                
            }

            return listToReturn.AsEnumerable<Forecast>();

        }

        public IEnumerable<Forecast> preGetForecastFromLaNLo(string latitude, string longtitude)
        { // Här ska cachening göras...


            return GetForecastFromLaNLo(latitude, longtitude);

        }

        internal Location getImageForLocationForecasts(Location theLocation)
        {
            foreach(var forecast in theLocation.Forecast){
                forecast.imageUrl = "http://api.yr.no/weatherapi/weathericon/1.1/?symbol=" + forecast.PictureName + ";content_type=image/svg%2Bxml";
            }
            return theLocation;
        }
    }
}
