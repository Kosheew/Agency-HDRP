namespace Loader
{
    public class LoaderPresenter
    {
        private LoaderView _loaderView;
        private SceneLoader _sceneLoader;
        
        public LoaderPresenter(LoaderView loaderView, SceneLoader sceneLoader)
        {
            _loaderView = loaderView;
            _sceneLoader = sceneLoader;


            _sceneLoader.OnSceneLoaded += OnUpdateProgress;
        }

        private void OnUpdateProgress(float progress)
        {
            _loaderView.LoadStart();
            _loaderView.UpdateProgress(progress);
        }
        
    }
    
}