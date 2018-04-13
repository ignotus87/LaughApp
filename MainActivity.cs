using System;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Ads;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Widget;
using Firebase.Analytics;
using Firebase.Iid;

namespace LaughApp
{
    [Activity(Label = "Laugh!", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const string TAG = "LaughAppMainActivity";
        private ImageButton _imageButton;
        private FirebaseAnalytics firebaseAnalytics;

        private int[] _imageResources = new int[]
        {
            Resource.Drawable.image1,
            Resource.Drawable.image2,
            Resource.Drawable.image3,
            Resource.Drawable.image4,
            Resource.Drawable.image5,
            Resource.Drawable.image6,
            Resource.Drawable.image7,
            Resource.Drawable.image8,
            Resource.Drawable.image9,
            Resource.Drawable.baby_laugh,
            Resource.Drawable.girl_laugh,
            Resource.Drawable.laughter_640,
            Resource.Drawable.image13
        };

        private int _previousImageIndex = -999;

        private MediaPlayer _player = null;

        private int[] _soundResources = new int[]
        {
            Resource.Raw.man_giggling,
            Resource.Raw.evil_laugh,
            Resource.Raw.kid_laugh,
            Resource.Raw.baby_boy_laugh,
            Resource.Raw.crowd_laughing,
            Resource.Raw.funny_boy_laugh,
            Resource.Raw.funny_kid_giggle,
            Resource.Raw.giggle_sound,
            Resource.Raw.goofy_laugh,
            Resource.Raw.jolly_laugh,
            Resource.Raw.kid_giggle_3x,
            Resource.Raw.kid_laughing_short,
            Resource.Raw.laugh_to_self,
            Resource.Raw.toddler_laugh
        };

        private int _previousSoundIndex = -999;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            HideActionBar();

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main); // Set our view from the "main" layout resource

            InitializeFirebase();

            InitializeBannerAd();

            InitializeImageButton();

            PlayRandomSound();
        }

        private void InitializeFirebase()
        {
            firebaseAnalytics = FirebaseAnalytics.GetInstance(this);
        }

        private void InitializeBannerAd()
        {
            var adID = @"ca-app-pub-1961552115246599~9626940797";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, adID);

            var adView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
        }

        private void InitializeImageButton()
        {
            _imageButton = FindViewById<ImageButton>(Resource.Id.imageButton1);

            _imageButton.Click += delegate
            {
                SetRandomImage();
                LogImageClick();
            };

            SetRandomImage();
        }

        private void LogImageClick()
        {
            Bundle bundle = new Bundle();
            bundle.PutString(FirebaseAnalytics.Param.ItemName, "Image clicked");
            firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.SelectContent, bundle);
        }

        private void HideActionBar()
        {
            Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            ActionBar.Hide();
        }

        private void SetRandomImage()
        {
            _imageButton.SetImageResource(GetNextImageResouce());
            PlayRandomSound();
        }

        private int GetRandomFromRange(int min, int max)
        {
            var random = new System.Random();
            return random.Next(min, max + 1);
        }

        private int GetNextImageResouce()
        {
            int nextImageIndex = -1;
            do
            {
                nextImageIndex = GetRandomFromRange(0, _imageResources.Length - 1);
            }
            while (nextImageIndex == _previousImageIndex);

            _previousImageIndex = nextImageIndex;

            return _imageResources[nextImageIndex];
        }

        private Android.Net.Uri GetNextSoundUri()
        {
            int nextSoundIndex = -1;
            do
            {
                nextSoundIndex = GetRandomFromRange(0, _soundResources.Length - 1);
            }
            while (nextSoundIndex == _previousSoundIndex);

            _previousSoundIndex = nextSoundIndex;

            return Android.Net.Uri.Parse("android.resource://" + this.ApplicationContext.PackageName + "/" + _soundResources[nextSoundIndex]);
        }

        private void PlayRandomSound()
        {
            var nextSoundUri = GetNextSoundUri();

            if (_player == null)
            {
                _player = MediaPlayer.Create(this, nextSoundUri);
            }
            else
            {
                _player.Reset();
                _player.SetDataSource(this, nextSoundUri);
                _player.Prepare();
            }
            _player.Start();
        }
    }
}

