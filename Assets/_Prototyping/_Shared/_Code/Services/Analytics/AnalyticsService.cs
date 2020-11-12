using System.Collections.Generic;
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

        private SceneHelper.SceneLoadAction m_Unload;

        #endregion // Inspector

        #region Log Variables

        private SimpleLog m_Logger;
        private ModelState m_ModelState;

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

            m_ModelState = new ModelState();

            m_Unload = Unload;
            SceneHelper.OnSceneUnload += m_Unload;
        }

        private void Unload(SceneBinding inScene, object inContext)
        {
            m_ModelState = null;
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
            if (m_ModelState.ScenarioId != scenarioId)
            {
                m_ModelState.ScenarioId = scenarioId;
            }

            RuleData ruleData = new RuleData(actorType, valueType, newValue);

            m_ModelState.PrevModelData = m_ModelState.CurrModelData;
            m_ModelState.CurrModelData = new ModelData(ruleData, m_ModelState.CurrTick);

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "scenario_id", scenarioId },
                { "curr_tick", m_ModelState.CurrTick },
                { "prev_value", prevValue },
                { "rule_data", new RuleData(actorType, valueType, newValue).DataString }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.modeling_behavior_change));
        }

        public void LogModelingTickChange(string clickLocation, string prevTick, string newTick)
        {
            m_ModelState.CurrTick = newTick;

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "scenario_id", m_ModelState.ScenarioId},
                { "click_location", clickLocation },
                { "prev_tick", prevTick },
                { "new_tick", newTick }
            };

            m_Logger.Log(new LogEvent(data, m_EventCategories.modeling_tick_change));
        }

        public void LogModelingSyncChange(string scenarioId, string prevSync, string newSync)
        {
            m_ModelState.CurrSync = newSync;

            if (m_ModelState.ScenarioId != scenarioId)
            {
                m_ModelState.ScenarioId = scenarioId;
            }

            if (m_ModelState.PrevModelData != null && m_ModelState.CurrModelData != null)
            {
                Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    { "scenario_id", scenarioId },
                    { "prev_sync", prevSync },
                    { "new_sync", newSync },
                    { "prev_model_data", m_ModelState.PrevModelData.DataString },
                    { "new_model_data", m_ModelState.CurrModelData.DataString }
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
}
