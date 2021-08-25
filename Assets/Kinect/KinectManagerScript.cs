using UnityEngine;
using System.Collections;
using Microsoft.Kinect.VisualGestureBuilder;
using Microsoft.Kinect;

public class KinectManagerScript : MonoBehaviour
{
    VisualGestureBuilderDatabase _dbGestures;
    Windows.Kinect.KinectSensor _kinect;
    VisualGestureBuilderFrameSource _gestureFrameSource;
    Windows.Kinect.BodyFrameSource _bodyFrameSource;
    VisualGestureBuilderFrameReader _gestureFrameReader;
    Windows.Kinect.BodyFrameReader _bodyFrameReader;
    Gesture _swipeUpDown; // наш жест
    Windows.Kinect.Body[] _bodies; // все пользователи, найденные Kinect'ом
    Windows.Kinect.Body _currentBody = null; //Текущий пользователь, жесты которого мы отслеживаем
    public string _getsureBasePath = "upDown.gbd"; //Путь до нашей обученной модели
    bool gestureDetected = false;
    public delegate void SimpleEvent();
    public static event SimpleEvent OnSwipeUpDown;
    void Start()
    {
        InitKinect();
    }


    void InitKinect()
    {
        _dbGestures = VisualGestureBuilderDatabase.Create(_getsureBasePath);
        _bodies = new Windows.Kinect.Body[6];
        _kinect = Windows.Kinect.KinectSensor.GetDefault();
        _kinect.Open();
        _gestureFrameSource = VisualGestureBuilderFrameSource.Create(_kinect, 0);

        foreach (Gesture gest in _dbGestures.AvailableGestures)
        {
            _gestureFrameSource.AddGesture(gest);
            if (gest.Name == "UpDownSwipe_Right")
            {
                _swipeUpDown = gest;
                Debug.Log("Added:" + gest.Name);
            }
        }
        _bodyFrameSource = _kinect.BodyFrameSource;
        _bodyFrameReader = _bodyFrameSource.OpenReader();
        _bodyFrameReader.FrameArrived += _bodyFrameReader_FrameArrived;

        _gestureFrameReader = _gestureFrameSource.OpenReader();
        _gestureFrameReader.IsPaused = true;
        _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
    }

    void _bodyFrameReader_FrameArrived(object sender, Windows.Kinect.BodyFrameArrivedEventArgs args)
    {
        var frame = args.FrameReference;
        using (var multiSourceFrame = frame.AcquireFrame())
        {
            multiSourceFrame.GetAndRefreshBodyData(_bodies); //обновляем данные о найденных людях
            _currentBody = null;
            foreach (var body in _bodies)
            {
                if (body != null && body.IsTracked)
                {
                    _currentBody = body; // для простоты берем первого найденного человека
                    break;
                }
            }
            if (_currentBody != null)
            {
                Debug.Log("_currentBody is not null");
                _gestureFrameSource.TrackingId = _currentBody.TrackingId;
                _gestureFrameReader.IsPaused = false;
            }
            else
            {
                Debug.Log("_currentBody is null");
                _gestureFrameSource.TrackingId = 0;
                _gestureFrameReader.IsPaused = true;
            }

        }
    }

    void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs args)
    {

        if (_gestureFrameSource.IsTrackingIdValid)
        {
            Debug.Log("Tracking id is valid, value = " + _gestureFrameSource.TrackingId);
            using (var frame = args.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    //using (var results = frame.DiscreteGestureResults)
                    var results = frame.DiscreteGestureResults;
                    if (results != null && results.Count > 0)
                    {
                        DiscreteGestureResult swipeUpDownResult;
                        results.TryGetValue(_swipeUpDown, out swipeUpDownResult);
                        Debug.Log("Result not null, conf = " + swipeUpDownResult.Confidence);

                        if (swipeUpDownResult.Confidence > 0.2)
                        {
                            if (!gestureDetected)
                            {
                                gestureDetected = true;
                                Debug.Log("Up Down Gesture");
                                if (OnSwipeUpDown != null)
                                    OnSwipeUpDown();
                            }
                        }
                        else
                        {
                            Debug.Log("False");

                            gestureDetected = false;
                        }
                    }
                }
            }
        }
    }


    void Update()
    {

    }
}