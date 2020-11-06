using System.Collections.Generic;
using System.Text;
using BeauData;
using BeauUtil;
using FieldDay;
using UnityEngine;

namespace ProtoAqua
{
    public partial class AnalyticsService : ServiceBehaviour
    {
        #region Inspector

        [SerializeField] private string m_AppId = "AQUALAB";
        [SerializeField] private int m_AppVersion = 1;

        #endregion // Inspector

        #region Log Variables

        private SimpleLog m_Logger;

        // TODO: Separate classes for cached information for each context
        // TODO: Clear cache on exit (reset)
        //        - SceneHelper.OnSceneUnload

        private string m_ScenarioId;
        private string m_CurrTick;
        private string m_CurrSync;

        private ModelData m_PrevModelData;
        private ModelData m_CurrModelData;

        private enum m_EventCategories
        {
            modeling_behavior_change,
            modeling_tick_change,
            modeling_sync_change,
            argumentation_response_click,
            argumentation_tab_click,
            argumentation_type_click,
            experimentation_tablet_click,
            experimentation_setup_click
        }

        #endregion // Log Variables

        #region IService

        protected override void OnRegisterService()
        {
            QueryParams queryParams = Services.Data.PeekQueryParams();
            m_Logger = new SimpleLog(m_AppId, m_AppVersion, queryParams);
        }

        protected override void OnDeregisterService()
        {
            m_Logger?.Flush();
            m_Logger = null;
        }

        public override FourCC ServiceId()
        {
            return ServiceIds.Analytics;
        }

        #endregion // IService

        #region Modeling

        public void LogModelingBehaviorChange(string scenarioId, string actorType, string valueType, string prevValue, string newValue)
        {
            if (m_ScenarioId != scenarioId)
            {
                m_ScenarioId = scenarioId;
            }

            RuleData ruleData = new RuleData(actorType, valueType, newValue);

            m_PrevModelData = m_CurrModelData;
            m_CurrModelData = new ModelData(ruleData, m_CurrTick);

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "scenario_id", scenarioId },
                { "curr_tick", m_CurrTick },
                { "prev_value", prevValue },
                { "rule_data", new RuleData(actorType, valueType, newValue).DataString }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.modeling_behavior_change));
        }

        public void LogModelingTickChange(string clickLocation, string prevTick, string newTick)
        {
            m_CurrTick = newTick;

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "scenario_id", m_ScenarioId},
                { "click_location", clickLocation },
                { "prev_tick", prevTick },
                { "new_tick", newTick }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.modeling_tick_change));
        }

        public void LogModelingSyncChange(string scenarioId, string prevSync, string newSync)
        {
            m_CurrSync = newSync;

            if (m_ScenarioId != scenarioId)
            {
                m_ScenarioId = scenarioId;
            }

            if (m_PrevModelData != null && m_CurrModelData != null)
            {
                Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    { "scenario_id", scenarioId },
                    { "prev_sync", prevSync },
                    { "new_sync", newSync },
                    { "prev_model_data", m_PrevModelData.DataString },
                    { "new_model_data", m_CurrModelData.DataString }
                };

                m_Logger.Log(new LogEvent(data, m_EventCategories.modeling_sync_change));
            }
        }

        #endregion // Modeling

        #region Argumentation

        public void LogArgumentationResponseClick(string argumentId, string responseId, string valid)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "argument_id", argumentId },
                { "response_id", responseId },
                { "valid", valid }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.argumentation_response_click));
        }

        public void LogArgumentationTabClick(string argumentId, string newTab)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "argument_id", argumentId },
                { "new_tab", newTab }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.argumentation_tab_click));
        }

        public void LogArgumentationTypeClick(string argumentId, string newType)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "argument_id", argumentId },
                { "new_type", newType }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.argumentation_type_click));
        }

        #endregion // Argumentation

        #region Experimentation

        public void LogExperimentationTabletClick(string clickType)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "click_type", clickType }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.experimentation_tablet_click));
        }

        // Call from ExperimentSetupPanelWorld.OnSetupSubmit? Need setup values
        public void LogExperimentationSetupClick(string tankType, string ecoType)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "tank_type", tankType },
                { "eco_type", ecoType }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.experimentation_setup_click));
        }

        #endregion // Experimentation

        #region Observation

        // TODO: scan start, scan stop, click on scanned object, click to move

        #endregion // Observation
    }

    #region Data Classes

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
   
    #endregion // Data Classes
}
