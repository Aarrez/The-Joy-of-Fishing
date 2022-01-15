using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineVirtualCamera CMcam;
    CinemachineFramingTransposer CMcamBody;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    BoatScript boatScript;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one Gamemanager");
            Destroy(instance.gameObject);
            instance = this;
        }
        Fadeimage.color = new Color(0, 0, 0, 255);
    }
    // Start is called before the first frame update
    void Start()
    {
        //CMcam = GetComponent<CinemachineVirtualCamera>();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        boatScript = FindObjectOfType<BoatScript>();
        CMcamBody = CMcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        UIScreenfadein();

    }

    // Update is called once per frame
    void Update()
    {

        if (moveCam == 2 && baitCam == false)
        {
            ShopCamTrue();
        }

        if (moveCam == 1 && baitCam == false)
        {
            ShopCamFalse();
        }

        if (moveCam == 3 && baitCam == true)
        {
            BaitCam();
        }

    }
    public void ChangeInteger()
    {
        moveCam = 2;
    }

    public void ChangeIntegerAgain()
    {
        moveCam = 1;
    }

    public void ShopCamTrue()
    {
        CMcam.Follow = ShoppeBoat;
        CMcamBody.m_TrackedObjectOffset.y = 3;
    }

    public void ShopCamFalse()
    {
        CMcam.Follow = Player;
        CMcamBody.m_TrackedObjectOffset.y = 3;
    }

    public void BaitCam()
    {
        Transform currenthook;
        currenthook = GameObject.Find("Hook(Clone)").GetComponent<Transform>();
            CMcam.Follow = currenthook;
            CMcamBody.m_TrackedObjectOffset.y = 0;
    }


//============================ ScreenFader ============================
    public Image Fadeimage;
    public GameObject FadeCanvas;
    public void UIScreenfadeout() 
    {
        StartCoroutine(FadeOutCR());
    }
    public void UIScreenfadein() 
    {
        StartCoroutine(FadeInCR());

    } 
        private IEnumerator FadeOutCR()
    {
        float duration = 1f; //0.5 secs
        float currentTime = 0f;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.MoveTowards(0f, 1f, currentTime/duration);
            Fadeimage.color = new Color(Fadeimage.color.r, Fadeimage.color.g, Fadeimage.color.b, alpha);
        }
        FadeCanvas.SetActive(true);
        yield break;
    }

        private IEnumerator FadeInCR()
    {
        float duration = 1f; //0.5 secs
        float currentTime = 0f;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.MoveTowards(1f, 0f, currentTime/duration);
            Fadeimage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        FadeCanvas.SetActive(false);
        yield break;
    }









//-----------------------------------------SpeechBubbles----------
    /// <summary>
	/// Character start font size.
	/// </summary>
	public int characterStartSize = 1;

	/// <summary>
	/// Character size animate speed.
	/// Unit: delta font size / second
	/// </summary>
	public float characterAnimateSpeed = 1000f;

	/// <summary>
	/// The bubble background (OPTIONAL).
	/// </summary>
	public Image bubbleBackground;

	/// <summary>
	/// Minimum height of background.
	/// </summary>
	public float backgroundMinimumHeight;

	/// <summary>
	/// Vertical margin (top + bottom) between label and background (OPTIONAL).
	/// </summary>
	public float backgroundVerticalMargin;

	/// <summary>
	/// A copy of raw text.
	/// </summary>
	private string _rawText;
	public string rawText {
		get { return _rawText; }
	}

	/// <summary>
	/// Processed version of raw text.
	/// </summary>
	private string _processedText;
	public string processedText {
		get { return _processedText; }
	}

	/// <summary>
	/// Set the label text.
	/// </summary>
	/// <param name="text">Text.</param>


    public void Set (string text) 
    {
		StopAllCoroutines();
		StartCoroutine(SetRoutine(text));
	}	

	/// <summary>
	/// Set the label text.
	/// </summary>
	/// <param name="text">Text.</param>
	public IEnumerator SetRoutine (string text) 
	{
		_rawText = text;
		yield return StartCoroutine(TestFit());
		yield return StartCoroutine(CharacterAnimation());
	}

	/// <summary>
	/// Test fit candidate text,
	/// set intended label height,
	/// generate processed version of the text.
	/// </summary>
	private IEnumerator TestFit () 
	{
		// prepare targets
		Text label = GetComponent<Text>();
		ContentSizeFitter fitter = GetComponent<ContentSizeFitter>();

		// change label alpha to zero to hide test fit
		float alpha = label.color.a;
		label.color = new Color(label.color.r, label.color.g, label.color.b, 0f);

		// configure fitter and set label text so label can auto resize height
		fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
		fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		label.text = _rawText;

		// need to wait for a frame before label's height is updated
		yield return new WaitForEndOfFrame();
		// make sure label is anchored to center to measure the correct height
		float totalHeight = label.rectTransform.sizeDelta.y;

		// (OPTIONAL) set bubble background
		if (bubbleBackground != null) {
			bubbleBackground.rectTransform.sizeDelta = new Vector2(
				bubbleBackground.rectTransform.sizeDelta.x, 
				Mathf.Max(totalHeight + backgroundVerticalMargin, backgroundMinimumHeight));
		}

		// now it's time to test word by word
		_processedText = "";
		string buffer = "";
		string line = "";
		float currentHeight = -1f;
		// yes, sorry multiple spaces
		foreach (string word in _rawText.Split(' ')) 
        {
			buffer += word + " ";
			label.text = buffer;
			yield return new WaitForEndOfFrame();
			if (currentHeight < 0f) 
            {
				currentHeight = label.rectTransform.sizeDelta.y;
			}
			if (currentHeight != label.rectTransform.sizeDelta.y) 
            {
				currentHeight = label.rectTransform.sizeDelta.y;
				_processedText += line.TrimEnd(' ') + "\n";
				line = "";
			}
			line += word + " ";
		}
		_processedText += line;

		// prepare fitter and label for character animation
		fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
		fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
		label.text = "";
		label.rectTransform.sizeDelta = new Vector2(label.rectTransform.sizeDelta.x, totalHeight);
		label.color = new Color(label.color.r, label.color.g, label.color.b, alpha);
	}

	private IEnumerator CharacterAnimation () 
	{
		// prepare target
		Text label = GetComponent<Text>();

		// go through character in processed text
		string prefix = "";
		foreach (char c in _processedText.ToCharArray()) 
        {
			// animate character size
			int size = characterStartSize;
			while (size < label.fontSize) {
				size += (int)(Time.deltaTime * characterAnimateSpeed);
				size = Mathf.Min(size, label.fontSize);
				label.text = prefix + "<size=" + size + ">" + c + "</size>";
				yield return new WaitForEndOfFrame();
			}
			prefix += c;
		}

		// set processed text
		label.text = _processedText;
	}

}
