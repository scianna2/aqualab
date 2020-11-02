using UnityEngine;
using ProtoCP;

namespace ProtoAqua.Experiment
{
    public class ExperimentSetupPanelWorld : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private PointerListener m_Proxy = null;
        [SerializeField] private SpriteAnimator m_Animation = null;

        #endregion // Inspector

        #region Unity Events

        private void Awake()
        {
            m_Proxy.onClick.AddListener((e) => Services.Events.Dispatch(ExperimentEvents.SetupPanelOn));

            Services.Events.Register(ExperimentEvents.SetupPanelOn, OnPanelOn, this)
                .Register(ExperimentEvents.SetupPanelOff, OnPanelOff, this)
                .Register(ExperimentEvents.SetupInitialSubmit, OnSetupSubmit, this)
                .Register(ExperimentEvents.ExperimentTeardown, OnExperimentTeardown, this);
        }

        private void OnDestroy()
        {
            Services.Events?.DeregisterAll(this);
        }

        #endregion // Unity Events

        private void OnPanelOn()
        {
            m_Animation.Pause();
            Services.Analytics.LogExperimentationTabletClick("OPEN");
        }

        private void OnPanelOff()
        {
            m_Animation.Restart();
            Services.Analytics.LogExperimentationTabletClick("CLOSE");
        }

        private void OnSetupSubmit()
        {
            gameObject.SetActive(false);
            Services.Analytics.LogExperimentationStart(ExperimentVars.TankType, ExperimentVars.EcoType);
        }

        private void OnExperimentTeardown()
        {
            gameObject.SetActive(true);
        }
    }
}