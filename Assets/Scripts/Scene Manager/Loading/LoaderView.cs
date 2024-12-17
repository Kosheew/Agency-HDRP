using UnityEngine;
using UnityEngine.UI;

namespace Loader
{
    public class LoaderView: MonoBehaviour
    {
        [SerializeField] private Slider progressLoading;
        [SerializeField] private GameObject loadingPanel;
        
        private LoaderPresenter _presenter;

        public void Inject(DependencyContainer container)
        {
            var loadingScene = container.Resolve<SceneLoader>();
            _presenter = new LoaderPresenter(this, loadingScene);
        }

        public void UpdateProgress(float progress)
        {
            progressLoading.SetValueWithoutNotify(progress);
        }

        public void LoadStart()
        {
            loadingPanel.SetActive(true);
        }
    }
}