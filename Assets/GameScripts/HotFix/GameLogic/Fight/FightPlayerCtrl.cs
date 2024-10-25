using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using DamageNumbersPro.Demo;
using TEngine;
using UnityEditor.Animations;
using UnityEngine;
using static GameLogic.PlayerProperty;

namespace GameLogic
{
    public class FightPlayerCtrl : BasePlayerCpt<BaseCptMono>, IBasePlayer
    {
        public DamageNumber popupPrefab;
        protected PlayerProperty mPro;
        protected GameObject mObj;
        protected PlayerAnimator mAnimatorCtr;
        protected Vector3 mTargetMovePostion;
        private RectTransform mTransform;
        public FightPlayerCtrl(int loc) : base(loc)
        {
        }
        protected FightPlayerMono Mono;

        public async void InitAsync(int loc, GameObject obj)
        {
            if(loc==1)
                mPro = BaseFightData.GetPlayerPro<PlayerProperty>(loc);
            else 
                mPro = BaseFightData.GetEnemyPlayerPro<PlayerProperty>(loc);
            Mono = obj.GetComponent<FightPlayerMono>();
            mTransform = obj.GetComponent<RectTransform>();
            Mono.transform.position = mPro.Position;
            Debug.Log(loc + " Init----mPro" + mPro.Position);
            //Mono.transform.localScale = new Vector3(2, 2, 2);
            Mono.mLoc = loc;

            AnimatorController ctrl =
                await GameModule.Resource.LoadAssetAsync<AnimatorController>(loc == 2
                    ? "ctrl_card2021"
                    : "ctrl_card1034");
            Mono.mAnimator.runtimeAnimatorController = ctrl;
            GameObject go = await GameModule.Resource.LoadAssetAsync<GameObject>("PopupNumber");
            popupPrefab = go.GetComponent<DamageNumber>();
            mAnimatorCtr = new PlayerAnimator(Mono.mAnimator);
            mAnimatorCtr.SetBool("IsWalking", true);

        }

        [PlayerMessenger(PlayerProperty.UpdatePlayerPro, PlayerProEnumKey.moveStatus)]
        protected void DoveStatus(int loc, string key, object data)
        {
            if(loc != mPro.Loc)return;
            
            if (mAnimatorCtr == null) return;
            EnumUtil.MoveStatus moveStatus = (EnumUtil.MoveStatus) data;
            mAnimatorCtr.SetBool("IsWalking", moveStatus == EnumUtil.MoveStatus.移动);
        }
        
        [PlayerMessenger(PlayerProperty.UpdatePlayerProVector, PlayerProEnumKey.position)]
        protected void DoMove(int loc, string key, Vector3 data)
        {
            if(loc != mPro.Loc)return;
            mTargetMovePostion = data;
            Mono.mRenderer.flipX = mTransform.position.x > mTargetMovePostion.x;
        }
        

        [PlayerMessenger(PlayerProperty.UpdatePlayerPro, PlayerProEnumKey.curSkill)]
        private void Skill(int loc, string key, object data)
        {
            if (mAnimatorCtr == null) return;
            //if (loc != Mono.mLoc) return;
            Skill skillItem = (Skill)data;

            if (skillItem != null)
            {
                mAnimatorCtr.SetInt("Skill", 1);
                mAnimatorCtr.DoTrigger();
            }
            else
            {
                mAnimatorCtr.SetInt("Skill", 0);
            }
        }


        [PlayerMessenger(PlayerProperty.UpdatePlayerProBool, PlayerProEnumKey.isDie)]
        private void Die(int loc, string key, bool data)
        {
            

        }


        [PlayerMessenger(PlayerProperty.UpdatePlayerProInt, PlayerProEnumKey.hurt)]
        private void Hurt(int loc, string key, int data)
        {
            
        }
        



        protected virtual void InitAnimator( GameObject weaponObj)
        {
            InitPlayAnim();
            
        }

        void InitPlayAnim()
        {
            
        }



        public void DoFixUpdate()
        {
        }

        
        public void DoUpdate()
        {
            //mTransform.position = mTargetMovePostion;
            //Debug.Log(mPro.Loc + "----------------" + mTransform.position);
            mTransform.position = Vector3.Lerp(mTransform.position, mTargetMovePostion, 1);
            if (Mono.mLoc > 1)//敌人
            {
                BasePlayerProperty target = BaseFightData.GetPlayerPro<BasePlayerProperty>(1);
                Mono.mRenderer.flipX = mTransform.position.x > target.Position.x;
            }
            if(DNP_InputHandler.GetLeftClick())
            {
                SpawnPopup(Mathf.Round(Random.Range(1, 10)));
            }
        }
        
        public void SpawnPopup(float number)
        {
            DamageNumber newPopup = popupPrefab.SpawnGUI(mTransform, new Vector2(0.5f,.5f), number); //Spawn DamageNumber     <-----     [REQUIRED]

            //You can do any change you want on the DamageNumber returned by the Spawn(...) function.
            if(Random.value < 0.5f)
            {
                newPopup.number *= 2;
            }
        }

        public override void Clear()
        {
            base.Clear();
        }

        public Transform GetTransform()
        {
            return mObj.transform;
        }

        public int GetLoc()
        {
            return 1;
        }

    }
}