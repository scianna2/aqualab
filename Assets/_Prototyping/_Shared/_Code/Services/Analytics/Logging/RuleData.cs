using System.Text;
using BeauUtil;

namespace ProtoAqua
{
    public class RuleData
    {
        public string DataString { get; set; }

        public RuleData(string organism, string valueType, string currValue)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{{\"organism\":\"{0}\"}},{{\"value_type\":\"{1}\"}},{{\"curr_value\":\"{2}\"}}", organism, valueType, currValue);
            DataString = stringBuilder.Flush();
        }
    }
}
