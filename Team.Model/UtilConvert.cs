using System;

namespace Team.Model
{
    public static class UtilConvert
    {
        public static string ObjToString(this object thisValue)
        {
            if (thisValue!=null)
            {
                return thisValue.ToString().Trim();
            }

            return "";
        }

        public static int ObjToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue==null)
            {
                return 0;
            }

            if (thisValue!=null && thisValue!=DBNull.Value&& int.TryParse(thisValue.ToString(),out reval))
            {
                return reval;
            }

            return reval;
        }
    }
}
