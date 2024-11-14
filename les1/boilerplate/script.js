import { HandLandmarker, FilesetResolver } from "https://cdn.jsdelivr.net/npm/@mediapipe/tasks-vision@0.10.0";

const video = document.getElementById("webcam")
const canvasElement = document.getElementById("output_canvas")
const canvasCtx = canvasElement.getContext("2d")
const enableWebcamButton = document.getElementById("webcamButton")
const logButton = document.getElementById("logButton")

let handLandmarker = undefined;
let webcamRunning = false;
let lastVideoTime = -1;
let results = undefined;

/********************************************************************
// CREATE THE POSE DETECTOR
********************************************************************/
const createHandLandmarker = async () => {
    const vision = await FilesetResolver.forVisionTasks("https://cdn.jsdelivr.net/npm/@mediapipe/tasks-vision@0.10.0/wasm");
    handLandmarker = await HandLandmarker.createFromOptions(vision, {
        baseOptions: {
            modelAssetPath: `https://storage.googleapis.com/mediapipe-models/hand_landmarker/hand_landmarker/float16/1/hand_landmarker.task`,
            delegate: "GPU"
        },
        runningMode: "VIDEO",
        numHands: 2
    });
    console.log("model loaded, you can start webcam")
    if (hasGetUserMedia()) {
        enableWebcamButton.addEventListener("click", (e) => enableCam(e))
        logButton.addEventListener("click", (e) => logAllHands(e))
    } 
}
const hasGetUserMedia = () => !!navigator.mediaDevices?.getUserMedia;

/********************************************************************
// START THE WEBCAM
********************************************************************/

function enableCam() {
    webcamRunning = true
    navigator.mediaDevices.getUserMedia({ video: true, audio: false }).then((stream) => {
        video.srcObject = stream
        video.addEventListener("loadeddata", () => {
            canvasElement.style.width = video.videoWidth
            canvasElement.style.height = video.videoHeight
            canvasElement.width = video.videoWidth
            canvasElement.height = video.videoHeight
            document.querySelector(".videoView").style.height = video.videoHeight + "px"
            predictWebcam()
        })
    })
}

/********************************************************************
// START PREDICTIONS    
********************************************************************/
async function predictWebcam() {
    results = await handLandmarker.detectForVideo(video, performance.now())

    canvasCtx.clearRect(0, 0, canvasElement.width, canvasElement.height);
    if (results.landmarks) {
        drawAllHands(results)
    }

    if (webcamRunning) {
       window.requestAnimationFrame(predictWebcam)
    }
}

/********************************************************************
// DRAW HANDS
********************************************************************/
function drawAllHands(results) {
    for (let i = 0; i < results.landmarks.length; i++) {
        let handmarks = results.landmarks[i]
        drawConnectors(canvasCtx, handmarks, HAND_CONNECTIONS, { color: "#03F600", lineWidth: 4 });
        drawLandmarks(canvasCtx, handmarks, { color: "#F40000", lineWidth: 3 })
    }
}

/********************************************************************
// LOG HAND COORDINATES IN THE CONSOLE
********************************************************************/
function logAllHands(){
    for (let hand of results.landmarks) {
        console.log(hand)
    }
}

createHandLandmarker()