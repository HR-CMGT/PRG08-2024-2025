In dit voorbeeld halen we de input (stream) van de webcam op en laden deze in een video element.

In de HTML plaats je een video element

```html
<video id="video" width="720" height="405" autoplay muted></video>
```

Met het attribuut `autoplay` zorg je ervoor dat het videobeeld direct zal afspelen als de stream aan het element is toegekend.

Vanuit javascript haal je eerst de webcam feed op en deze ken je toe aan het video element

```javascript
if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
  navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
    video.srcObject = stream;
    video.onloadedmetadata = () => {
      video.play();
    };
  });
}
```

<br><br><br>

### Opties
Met de functie `getUserMedia(...)` wordt er aan de bezoeker toestemming gevraagd om de media input te gebruiken. Hier kunnen verschillende [opties](https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia#parameters) aan meegegeven worden.


```js
getUserMedia({
  audio: true,
  video: { width: 1280, height: 720 },
});
```

```js
getUserMedia({
    audio: false, 
    video: {
      facingMode: "user",               // User-facing camera
      width: { ideal: 640 },            // Ideal width for small landscape format
      height: { ideal: 360 },           // Ideal height for small landscape format
      aspectRatio: { ideal: 16 / 9 },   // Landscape aspect ratio
    }
});
```

