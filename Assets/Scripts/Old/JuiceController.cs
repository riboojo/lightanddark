using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class JuiceController : MonoBehaviour
{
    private static JuiceController instance;
    public static JuiceController GetInstance { get { return instance; } }

    [SerializeField]
    Animator screen;

    [SerializeField]
    PostProcessVolume post;

    [SerializeField]
    Animator frame;

    [SerializeField]
    AnimationController frameAnim;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            Debug.LogError("Instance of JuiceController has already been created!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PerformScreenshake()
    {
        screen.SetTrigger("shake");
    }

    public void PerformFinishLevel()
    {
        frame.SetTrigger("finish");
    }

    public void PerformNewLevel()
    {
        frame.SetTrigger("start");
    }

    public bool IsFrameIdle()
    {
        return !frameAnim.isLevelCompleteAnimationStarted & !frameAnim.isLevelCompleteAnimationStarted & !frameAnim.isNewLevelAnimationStarted & !frameAnim.isNewLevelAnimationFinished;
    }

    public bool IsPerformFinishLevelRunning()
    {
        return frameAnim.isLevelCompleteAnimationStarted;
    }

    public bool IsPerformFinishLevelFinished()
    {
        return frameAnim.isLevelCompleteAnimationFinished;
    }

    public bool IsPerformNewLevelRunning()
    {
        return frameAnim.isNewLevelAnimationStarted;
    }

    public bool IsPerformNewLevelFinished()
    {
        return frameAnim.isNewLevelAnimationFinished;
    }

    public bool IsShowTutorialRunning()
    {
        return frameAnim.isShowTutorialStarted;
    }

    public bool IsShowTutorialFinished()
    {
        return frameAnim.isShowTutorialFinished;
    }

    public void BlurBackground()
    {
        DepthOfField depth;
        if (post.profile.TryGetSettings<DepthOfField>(out depth))
        {
            for (float i = 10.0f; i >= 0.1f; i-=0.001f)
            {
                depth.focusDistance.value = i;
            }
        }
    }

    public void FocusBackground()
    {
        DepthOfField depth;
        if (post.profile.TryGetSettings<DepthOfField>(out depth))
        {
            for (float i = 0.1f; i <= 10.0f; i+=0.001f)
            {
                depth.focusDistance.value = i;
            }
        }
    }
}
