using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingProtoV3
{
    static class ProtoUtils
    {
        public static Answer CreateOkAnswer()
        {
            Answer answer = new Answer { Type = Answer.Types.Type.Ok };
            return answer;
        }
        public static Answer CreateDynamicUpdateShowListAnswer(List<CommonDomain.Show> shows)
        {
            Answer answer = new Answer { Type = Answer.Types.Type.DynamicFullShowList};
            foreach(CommonDomain.Show show in shows)
            {
                NetworkingProtoV3.Show protoshow = new NetworkingProtoV3.Show 
                { 
                    Id=show.GetId(), 
                    Address = show.GetAddress(), 
                    ArtistName = show.GetArtistName(), 
                    AvailableSeats = show.GetAvailableSeats(), 
                    ShowName = show.GetShowName(), 
                    SoldSeats = show.GetSoldSeats(), 
                    Time = ConvertDateTimeToJava(show.GetStartTime()) 
                };
                answer.Shows.Add(protoshow);
            }
            return answer;
        }

        public static Answer CreateDynamicFilteredShowListAnswer(List<CommonDomain.Show> shows)
        {
            Answer answer = new Answer { Type = Answer.Types.Type.DynamicFilteredShowList };
            foreach (CommonDomain.Show show in shows)
            {
                NetworkingProtoV3.Show protoshow = new NetworkingProtoV3.Show 
                { 
                    Id = show.GetId(), 
                    Address = show.GetAddress(), 
                    ArtistName = show.GetArtistName(), 
                    AvailableSeats = show.GetAvailableSeats(), 
                    ShowName = show.GetShowName(), 
                    SoldSeats = show.GetSoldSeats(), 
                    Time = ConvertDateTimeToJava(show.GetStartTime()) 
                };
                answer.Shows.Add(protoshow);
            }
            return answer;
        }

        public static CommonDomain.User GetUserFromLogin(Request request)
        {
            CommonDomain.User user = new CommonDomain.User(request.User.Username, request.User.Password);
            return user;
        }

        public static Answer CreateErrorAnswer(string message)
        {
            Answer answer = new Answer { Type = Answer.Types.Type.Error, Error = message };
            return answer;
        }

        public static string GetUsernameFromLogout(Request request)
        {
            string username = request.Username;
            return username;
        }

        public static CommonDomain.Sale GetSaleFromRequest(Request request)
        {
            CommonDomain.Sale sale = new CommonDomain.Sale(request.Sale.BuyerName, request.Sale.TicketsBought, request.Sale.ShowId);
            return sale;
        }

        private static string ConvertDateTimeToJava(DateTime time)
        {
            string s = time.Year.ToString();
            s += "-";
            if (time.Month < 10)
                s += "0";
            s += time.Month.ToString();
            s += "-";
            if (time.Day < 10)
                s += "0";
            s += time.Day.ToString();
            s += "T";
            if (time.Hour < 10)
                s += "0";
            s += time.Hour.ToString();
            s += ":";
            if (time.Minute < 10)
                s+="0";
            s += time.Minute.ToString();
            s += ":";
            if (time.Second < 10)
                s += "0";
            s += time.Second.ToString();
            s += ".";
            s+=time.Millisecond.ToString();
            s += "000000";
            return s;
        }

        public static Answer CreateFullShowListAnswer(List<CommonDomain.Show> shows)
        {
            Answer answer = new Answer { Type = Answer.Types.Type.FullShowList };
            foreach (CommonDomain.Show show in shows)
            {
                NetworkingProtoV3.Show protoshow = new NetworkingProtoV3.Show {
                    Id=show.GetId(), 
                    Address = show.GetAddress(), 
                    ArtistName = show.GetArtistName(), 
                    AvailableSeats = show.GetAvailableSeats(),
                    ShowName = show.GetShowName(), 
                    SoldSeats = show.GetSoldSeats(),
                    Time = ConvertDateTimeToJava(show.GetStartTime())
                };
                answer.Shows.Add(protoshow);
            }
            return answer;
        }

        public static DateTime GetDateFromFilteredRequest(Request request)
        {
            DateTime time = DateTime.Parse(request.Datetime);
            return time;
        }

        public static Answer CreateFilteredShowListAnswer(List<CommonDomain.Show> filteredShows)
        {

            Answer answer = new Answer { Type = Answer.Types.Type.FilteredShowList };
            foreach (CommonDomain.Show show in filteredShows)
            {
                NetworkingProtoV3.Show protoshow = new NetworkingProtoV3.Show
                {
                    Id = show.GetId(),
                    Address = show.GetAddress(),
                    ArtistName = show.GetArtistName(),
                    AvailableSeats = show.GetAvailableSeats(),
                    ShowName = show.GetShowName(),
                    SoldSeats = show.GetSoldSeats(),
                    Time = ConvertDateTimeToJava(show.GetStartTime())
                };
                answer.Shows.Add(protoshow);
            }
            return answer;
        }
    }
}
