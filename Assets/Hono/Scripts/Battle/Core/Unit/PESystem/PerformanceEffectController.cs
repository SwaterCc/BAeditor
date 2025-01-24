using System.Threading;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 演出效果控制器
    /// </summary>
    public class PerformanceEffectController
    {
        public Actor Self { get; }

        /// <summary>
        /// 加载取消总key
        /// </summary>
        public CancellationTokenSource MainCancelToken { get; } = new();

        /// <summary>
        /// PE模板
        /// </summary>
        public PerformanceEffectsPlayer PEPlayer { get; private set; }

        /// <summary>
        /// 模型配置
        /// </summary>
        public PETemplate PETemplate { get; private set; }

        /// <summary>
        /// Performance Effect Templates 演出效果模板
        /// </summary>
        public int CurrentPETemplateId;

        public PerformanceEffectController(Actor actor)
        {
            Self = actor;
        }

        public async void Init(PerformanceEffectsPlayer model)
        {
            if (model != null)
            {
                PEPlayer = model;
                return;
            }

            var gameObject = await UPool.Instance.Get(PETemplate.model, MainCancelToken);

            if (gameObject == null)
            {
                return;
            }

            PEPlayer = gameObject.GetComponent<PerformanceEffectsPlayer>();

            PEPlayer?.OnInit(this);
            //Self.ModelLoadFinishCallback?.Invoke(Self);
        }

        public void Tick(float dt)
        {
            if (PEPlayer == null) return;

            PEPlayer.transform.localPosition = Self.Pos;
            PEPlayer.transform.localRotation = Self.Rot;
            PEPlayer.OnTick(dt);
        }

        public void Clear()
        {
            MainCancelToken.Cancel();
            if (PEPlayer != null)
            {
                //用池回收
                PEPlayer.Recycle();
            }
        }
    }
}