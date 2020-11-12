using System.Text;
using BeauUtil;

namespace ProtoAqua
{
    // TODO: Could potentially log state of all actors/values in model, rather than only the most recently updated
    public class ModelData
    {
        public RuleData UpdatedRuleData { get; }
        public string DataString { get; set; }

        public ModelData(RuleData updatedRuleData, string currTick)
        {
            UpdatedRuleData = updatedRuleData;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{{\"updated_rule_data\":\"{0}\"}},{{\"curr_tick\":\"{1}\"}}", updatedRuleData.DataString, currTick);
            DataString = stringBuilder.Flush();
        }
    }
}
