// using UnityEngine;
// using BeauUtil;
// using UnityEngine.UI;

// namespace Aqua
// {
//     public class JobToDoDisplay : MonoBehaviour
//     {
//         #region Inspector

//         [SerializeField, Required] private LocText m_NameLabel = null;
//         [SerializeField] private Image m_Icon = null;
//         #endregion // Inspector

//         public void Populate(JobDesc inJob, PlayerJobStatus inStatus = PlayerJobStatus.NotStarted)
//         {
//             if (!inJob)
//             {
//                 m_NameLabel.SetText(null);

//                 if (m_PosterLabel)
//                     m_PosterLabel.SetText(null);
                
//                 if (m_DescriptionLabel)
//                     m_DescriptionLabel.SetText(null);

//                 if (m_Icon)
//                     m_Icon.gameObject.SetActive(false);
                
//                 for(int i = 0; i < m_Rewards.Length; ++i)
//                     m_Rewards[i].gameObject.SetActive(false);
//                 for(int i = 0; i < m_Difficulties.Length; ++i)
//                     m_Difficulties[i].gameObject.SetActive(false);

//                 if (m_NoRewardsDisplay)
//                     m_NoRewardsDisplay.gameObject.SetActive(false);

//                 if (m_CompletedDisplay)
//                     m_CompletedDisplay.gameObject.SetActive(false);

//                 return;
//             }

//             m_NameLabel.SetText(inJob.NameId());
            
//             if (m_PosterLabel)
//                 m_PosterLabel.SetText(inJob.PosterId());
            
//             if (m_DescriptionLabel)
//                 m_DescriptionLabel.SetText(inJob.DescId());

//             if (m_Icon)
//             {
//                 var jobIcon = inJob.Icon();
//                 if (jobIcon != null)
//                 {
//                     m_Icon.sprite = jobIcon;
//                     m_Icon.gameObject.SetActive(true);
//                 }
//                 else
//                 {
//                     m_Icon.gameObject.SetActive(false);
//                     m_Icon.sprite = null;
//                 }
//             }
            
//             if (m_Rewards.Length > 0)
//             {
//                 int rewardCount = 0;
//                 if (inJob.CashReward() > 0)
//                 {
//                     PopulateReward(rewardCount++, GameConsts.CashId, inJob.CashReward());
//                 }
//                 if (inJob.GearReward() > 0)
//                 {
//                     PopulateReward(rewardCount++, GameConsts.GearsId, inJob.GearReward());
//                 }

//                 for(int i = rewardCount; i < m_Rewards.Length; ++i)
//                 {
//                     m_Rewards[i].gameObject.SetActive(false);
//                 }

//                 if (m_NoRewardsDisplay)
//                     m_NoRewardsDisplay.gameObject.SetActive(rewardCount == 0);
//             }

//             if (m_Difficulties.Length > 0)
//             {
//                 for(int i = 0; i < m_Difficulties.Length; ++i)
//                 {
//                     int difficulty = inJob.Difficulty((ScienceActivityType) i);
//                     m_Difficulties[i].Display(difficulty);
//                 }
//             }

//             if (m_CompletedDisplay)
//                 m_CompletedDisplay.gameObject.SetActive(inStatus == PlayerJobStatus.Completed);
//         }
//     }
// }