using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Kinect.VisualGestureBuilder;
using Microsoft.Kinect;

    public class KinectManager : MonoBehaviour
    {

        VisualGestureBuilderDatabase _dbGestures;
        Windows.Kinect.KinectSensor _kinect;
        VisualGestureBuilderFrameSource _gestureFrameSource;
        Windows.Kinect.BodyFrameSource _bodyFrameSource;
        VisualGestureBuilderFrameReader _gestureFrameReader;
        Windows.Kinect.BodyFrameReader _bodyFrameReader;
        Gesture _GLeft; // наш жест
        Gesture _GRight; // наш жест
        Gesture _GUp; // наш жест
        Gesture _GDown; // наш жест
        Gesture _Exit; // наш жест
        Windows.Kinect.Body[] _bodies; // все пользователи, найденные Kinect'ом
        Windows.Kinect.Body _currentBody = null; //Текущий пользователь, жесты которого мы отслеживаем
        public string _getsureBasePath = "Right2.gbd"; //Путь до нашей обученной модели

        bool gestureDetectedLeft = false;
        bool gestureDetectedRight = false;
        bool gestureDetectedJump = false;
        bool gestureDetectedDown = false;
        bool gestureDetectedStop = false;
        bool gestureDetectedStopJump = false;
        bool gestureDetectedStopDown = false;
        bool gestureDetectedExit = false;
        bool gestureDetectedStopExit = false;





        public delegate void SimpleEvent();
        public static event SimpleEvent OnSwipeUpDown;
        public static event SimpleEvent Up;
        public static event SimpleEvent Down;
        public static event SimpleEvent Left;
        public static event SimpleEvent Right;
        public static event SimpleEvent Stop;
        public static event SimpleEvent StopUp;
        public static event SimpleEvent StopDown;
        public static event SimpleEvent Exit;
        public static event SimpleEvent StopExit;
        // Start is called before the first frame update
        void Start()
        {
            Controll.Start();
            InitKinect();
        }
        void Awake()
        {

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
                if (gest.Name == "Left")
                {
                    _gestureFrameSource.AddGesture(gest);
                    _GLeft = gest;
                    // Debug.Log("Added:" + gest.Name);
                }
                if (gest.Name == "Right" || gest.Name == "Right_Right")
                {
                    _gestureFrameSource.AddGesture(gest);
                    _GRight = gest;
                    //Debug.Log("Added:" + gest.Name);
                }
                if (gest.Name == "Jump")
                {
                    _gestureFrameSource.AddGesture(gest);
                    _GUp = gest;
                    // Debug.Log("Added:" + gest.Name);
                }
                if (gest.Name == "Down" || gest.Name == "seet")
                {
                    _gestureFrameSource.AddGesture(gest);
                    _GDown = gest;
                    //Debug.Log("Added:" + gest.Name);
                }
                if (gest.Name == "Exit")
                {
                    _gestureFrameSource.AddGesture(gest);
                    _Exit = gest;
                    //Debug.Log("Added:" + gest.Name);
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
                    //              Debug.Log("_currentBody is not null");
                    _gestureFrameSource.TrackingId = _currentBody.TrackingId;
                    _gestureFrameReader.IsPaused = false;
                }
                else
                {
                    // Debug.Log("_currentBody is null");
                    _gestureFrameSource.TrackingId = 0;
                    _gestureFrameReader.IsPaused = true;
                }

            }
        }

        void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs args)
        {

            if (_gestureFrameSource.IsTrackingIdValid)
            {
                //Debug.Log("Tracking id is valid, value = " + _gestureFrameSource.TrackingId);
                using (var frame = args.FrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        //using (var results = frame.DiscreteGestureResults)
                        var results = frame.DiscreteGestureResults;
                        if (results != null && results.Count > 0)
                        {
                            DiscreteGestureResult swipeUpDownResult;
                            DiscreteGestureResult leftResult;
                            DiscreteGestureResult rightResult;
                            DiscreteGestureResult upResult;
                            DiscreteGestureResult downResult;
                            DiscreteGestureResult exitResult;
                            results.TryGetValue(_GLeft, out leftResult);
                            results.TryGetValue(_GRight, out rightResult);
                            results.TryGetValue(_GUp, out upResult);
                            results.TryGetValue(_GDown, out downResult);
                            results.TryGetValue(_Exit, out exitResult);
                            // Debug.Log("Result not null, leftResult = " + leftResult.Confidence);
                            // Debug.Log("Result not null, rightResult = " + rightResult.Confidence);
                            Debug.Log("Result not null, upResult = " + upResult.Confidence);
                            Debug.Log("Result not null, down  = " + downResult.Confidence);
                            Debug.Log("Result not null, exitResult  = " + exitResult.Confidence);

                            if (leftResult.Confidence > 0.3)
                            {
                                if (!gestureDetectedLeft)
                                {
                                    gestureDetectedLeft = true;
                                    //Debug.Log("Left Gesture");

                                    Left?.Invoke();

                                }
                            }
                            else
                            {
                                //Debug.Log("False");

                                gestureDetectedLeft = false;
                            }


                            if (rightResult.Confidence > 0.3)
                            {
                                if (!gestureDetectedRight)
                                {
                                    gestureDetectedRight = true;
                                    if (StopDown != null) Debug.Log("Есть");
                                    else Debug.Log("Нету");

                                    Right?.Invoke();
                                }
                                Debug.Log("Right Gesture");
                            }
                            else
                            {
                                //Debug.Log("False");

                                gestureDetectedRight = false;
                            }
                            if ((rightResult.Confidence <= 0.5) && (leftResult.Confidence <= 0.5))
                            {

                                if (!gestureDetectedStop)
                                {
                                    gestureDetectedStop = true;
                                    Stop?.Invoke();

                                    //Debug.Log("Stop Gesture");
                                    //if (Stop != null)
                                    //Controll.horizontal_move = 0 ;
                                }
                            }

                            else
                            {
                                //Debug.Log("False");

                                gestureDetectedStop = false;
                            }


                            if (upResult.Confidence > 0.3)
                            {

                                if (!gestureDetectedJump)
                                {

                                    gestureDetectedJump = true;
                                    Up?.Invoke();

                                }
                            }
                            else
                            {
                                gestureDetectedJump = false;
                            }
                            if (upResult.Confidence <= 0.3)
                            {

                                if (!gestureDetectedStopJump)
                                {

                                    gestureDetectedStopJump = true;
                                    StopUp?.Invoke();

                                }
                            }
                            else
                            {
                                gestureDetectedStopJump = false;
                            }
                            if (downResult.Confidence > 0.6)
                            {

                                if (!gestureDetectedDown)
                                {
                                    gestureDetectedDown = true;
                                    Down?.Invoke();
                                    //Debug.Log("Up  Gesture");

                                }
                            }
                            else
                            {
                                gestureDetectedDown = false;

                            }
                            if (downResult.Confidence <= 0.6)
                            {

                                if (!gestureDetectedStopDown)
                                {

                                    // Debug.Log("False");

                                    gestureDetectedStopDown = true;
                                    StopDown?.Invoke();
                                }

                            }
                            else
                            {
                                gestureDetectedStopDown = false;

                            }


                            if (exitResult.Confidence > 0.5)
                            {

                                if (!gestureDetectedExit)
                                {
                                    gestureDetectedExit = true;
                                    Exit?.Invoke();
                                    //Debug.Log("Up  Gesture");

                                }
                            }
                            else
                            {
                                gestureDetectedExit = false;

                            }
                            if (exitResult.Confidence <= 0.3)
                            {

                                if (!gestureDetectedStopExit)
                                {

                                    // Debug.Log("False");

                                    gestureDetectedStopExit = true;
                                    StopExit?.Invoke();
                                }

                            }
                            else
                            {
                                gestureDetectedStopExit = false;

                            }
                        }
                    }
                }
            }
        }


        public void Clear()
        {
            _kinect.Close();
        Windows.Kinect.KinectSensor.GetDefault().Close();
            _dbGestures = null;
            _gestureFrameSource = null;
            _bodyFrameSource = null;
            _gestureFrameReader = null;
            _bodyFrameReader = null;
            _GLeft = null; // наш жест
            _GRight = null; // наш жест
            _GUp = null; // наш жест
            _GDown = null; // наш жест
            _Exit = null;
            _bodies = null; // все пользователи, найденные Kinect'ом
            _currentBody = null; //Текущий пользователь, жесты которого мы отслеживаем
            _getsureBasePath = "";
            gestureDetectedLeft = false;
            gestureDetectedRight = false;
            gestureDetectedJump = false;
            gestureDetectedDown = false;
            gestureDetectedStop = false;
            gestureDetectedStopJump = false;
            gestureDetectedStopDown = false;
            gestureDetectedExit = false;
            gestureDetectedStopExit = false;

            OnSwipeUpDown = null;
            Up = null;
            Down = null;
            Left = null;
            Right = null;
            Stop = null;
            StopUp = null;
            StopDown = null;
            Exit = null;
            StopExit = null;






        }
    
    }


