using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    [SerializeField] Image progressBar;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess() 
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false ; // 로딩이 90퍼때 멈추게함 = 로딩화면 보여주기 너무 짧으면 있는 의미가 없다고함 ,
                                          // 에셋번들 리소스 받아오는 시간확보 (씬의 로딩이 더빠를수 있음)

        float timer = 0f;
        while (!op.isDone) // op가 끝나지않으면 반복
        {
            yield return null; //제어권을 돌려줘서 로딩바가 차게해줌
            if (op.progress < 0.9f) //로딩이 90% 미만이면 로딩바 업데이트해줌    
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 0.9에서 1로 1초에걸쳐채움
                if (progressBar.fillAmount >= 1f) 
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
