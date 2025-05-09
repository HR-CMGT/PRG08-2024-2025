# AI Spraak

Voor de meeste van de text-to-speech of speech-to-text API's heb je een eigen API key nodig.

## Code examples

- Whisper (speech to text) met Langchain en Azure
- OpenAI Fetch API (speech to text en text to speech)

## APIs

- [OpenAI](https://platform.openai.com/docs/api-reference/chat/create?lang=node.js)
- [Elevenlabs](https://elevenlabs.io/docs/api-reference/text-to-speech/convert)
- [Spark](https://sparkaudio.github.io/spark-tts)
- [Sesame](https://huggingface.co/sesame/csm-1b)
- [Kokoro](https://kokorottsai.com)

<br><bR><br>

## Whisper Langchain

Hiervoor heb je je eigen OpenAI key nodig in je `.env` file.

```js
import { OpenAIWhisperAudio } from "@langchain/community/document_loaders/fs/openai_whisper_audio";
//
// OPENAI WHISPER
//
async function openAIWhisper() {
    const filePath = "./hello.mp3";
    const options = { apiKey: process.env.OPENAI_API_KEY }
    const loader = new OpenAIWhisperAudio(filePath, options);
    const docs = await loader.load();
    console.log(docs[0].pageContent);
}
```

<br><bR><br>

## Azure Whisper

Langchain heeft hier geen ondersteuning voor, maar je kan Azure Whisper toch aanroepen via een omweg. 
Bij deployments geven we *'deploy-whisper'* als model mee.

```js
import { OpenAIWhisperAudio } from "@langchain/community/document_loaders/fs/openai_whisper_audio";

async function azureWhisper() {
    const filePath = "audio.mp3";

    const loader = new OpenAIWhisperAudio(filePath, {
        transcriptionCreateParams: { language: "nl" },
        clientOptions: {
            apiKey: process.env.AZURE_OPENAI_API_KEY,
            baseURL: `https://${process.env.AZURE_OPENAI_API_INSTANCE_NAME}.openai.azure.com/openai/deployments/deploy-whisper`,
            defaultQuery: { 'api-version': process.env.AZURE_OPENAI_API_VERSION },
            defaultHeaders: { 'api-key': process.env.AZURE_OPENAI_API_KEY },
        },
    });

    const docs = await loader.load();
    console.log(docs[0].pageContent);
}

await azureWhisper();
```
<br><bR><br>

# OpenAI Fetch API

Als je via `fetch` de OpenAI API aanroept heb je ook toegang tot de speech API's. Nu hoef je geen library's te installeren.

## Speech to text

In dit voorbeeld uploaden we een audio file via een Form, deze audio sturen we naar OpenAI om er tekst van te maken. Het form zou je kunnen vervangen door een "record" button die live audio opneemt.

```html
<input type="file" id="audioFile" accept="audio/*" />
<button onclick="uploadAudio()">Transcribe</button>
```

```js
async function uploadAudio() {
  const fileInput = document.getElementById('audioFile');
  const file = fileInput.files[0]; // This will be hello.mp3 or another audio file
  const apiKey = 'your-openai-api-key'; // Replace this with your actual key

  const formData = new FormData();
  formData.append('file', file);
  formData.append('model', 'whisper-1');

  const response = await fetch('https://api.openai.com/v1/audio/transcriptions', {
    method: 'POST',
    headers: { Authorization: `Bearer ${apiKey}` },
    body: formData,
  });

  const result = await response.json();
  console.log(result);
}
```

## Text to Speech

Je kan ook via `fetch` audio files genereren van tekst. Je kan een stem meegeven. Het opslaan van die files verschilt afhankelijk van de omgeving: browser of node server.

#### Browser

```js
async function speak() {
  const text = "Hamsters of the world, unite!";
  const apiKey = 'your-openai-api-key';

  const response = await fetch('https://api.openai.com/v1/audio/speech', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${apiKey}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      model: 'tts-1', // or 'tts-1-hd' for higher quality
      input: text,
      voice: 'nova' // voices: 'nova', 'shimmer', 'echo', etc.
    })
  });

  const audioBlob = await response.blob();

  // Optional: play it directly
  const audioUrl = URL.createObjectURL(audioBlob);
  const audio = new Audio(audioUrl);
  audio.play();

  // Optional: Download as file
  const a = document.createElement('a');
  a.href = audioUrl;
  a.download = 'speech.mp3';
  a.click();
}
```
#### Node server

```js
import { writeFile } from 'fs/promises';
const apiKey = 'your-openai-api-key';
const text = "Hamsters of the world, unite!";

const response = await fetch("https://api.openai.com/v1/audio/speech", {
  method: "POST",
  headers: {
    "Authorization": `Bearer ${apiKey}`,
    "Content-Type": "application/json",
  },
  body: JSON.stringify({
    model: "tts-1",        // or "tts-1-hd"
    input: text,
    voice: "nova",         // 'nova', 'shimmer', 'echo', etc.
  }),
});

if (!response.ok) {
  const err = await response.text();
  throw new Error(`OpenAI API error: ${response.status} ${err}`);
}

const buffer = Buffer.from(await response.arrayBuffer());
await writeFile("speech.mp3", buffer);
console.log("✅ Speech saved to speech.mp3");
```

#### Bronnen

- Kijk ook naar [openAi FM](https://www.openai.fm/)
