# AI Audio

- Whisper (speech > text)
- TTS (text > speech)

<Br><br><br>

### Whisper

Je kan een mp3 file naar *Whisper* sturen om spraak om te zetten naar tekst. Dit werkt het makkelijkst als je rechtstreeks OpenAI aanroept met je eigen OpenAI api key:

```js
import { OpenAIWhisperAudio } from "@langchain/community/document_loaders/fs/openai_whisper_audio";
async function openAIWhisper() {
    const filePath = "./hello.mp3";
    const loader = new OpenAIWhisperAudio(filePath, { apiKey: process.env.OPENAI_API_KEY });
    const docs = await loader.load();
    console.log(docs[0].pageContent);
}
```
- [Bekijk hier het voorbeeld in de OpenAI docs](https://platform.openai.com/docs/guides/speech-to-text)

### Azure Whisper

De Azure server heeft ook een whisper model, hiervoor kan je de azure api key gebruiken. De configuratie moet je helaas handmatig opstellen:

```js
async function azureWhisper() {
    const azureOpenAIConfig = {
        azureOpenAIApiKey: process.env.AZURE_OPENAI_API_KEY,
        azureOpenAIApiInstanceName: process.env.INSTANCE_NAME,
        azureOpenAIApiDeploymentName: "deploy-whisper", 
        azureOpenAIApiVersion: process.env.OPENAI_API_VERSION, 
    };
    const loader = new OpenAIWhisperAudio("./hello.mp3", {
        transcriptionCreateParams: { language: "en" },
        clientOptions: {
            apiKey: azureOpenAIConfig.azureOpenAIApiKey,
            baseURL: `https://${azureOpenAIConfig.azureOpenAIApiInstanceName}.openai.azure.com/openai/deployments/${azureOpenAIConfig.azureOpenAIApiDeploymentName}`,
            defaultQuery: { 'api-version': azureOpenAIConfig.azureOpenAIApiVersion },
            defaultHeaders: { 'api-key': azureOpenAIConfig.azureOpenAIApiKey },
        },
    });

    const docs = await loader.load();
    console.log(docs[0].pageContent);
}
```
<br><br><br>

## Text to speech

Er zijn veel TTS modellen beschikbaar, zoals [OpenAI](https://platform.openai.com/docs/guides/text-to-speech) en [Elevenlabs](https://elevenlabs.io/docs/api-reference/text-to-speech/convert). De CMGT Azure hosting heeft geen TTS model. 

Je kan openai installeren via `npm install openai`, of je kan de `REST API` van OpenAI aanroepen met `fetch`. Hieronder een voorbeeld van de REST API. 

```js
import fs from 'fs/promises';

async function textToSpeech(text, voice = "alloy", apiKey) {
    if (!text || text.length > 4096) {
        console.error("Text input is either empty or exceeds 4096 characters.");
        return false;
    }
    const payload = { model: "tts-1", voice: voice, input: text };
    const response = await fetch("https://api.openai.com/v1/audio/speech", {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${apiKey}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
    });

    if (response.ok) {
        const audioData = await response.arrayBuffer();
        return new Uint8Array(audioData);
    } else {
        const errorData = await response.text();
        console.error(`Error: ${response.status} - ${errorData}`);
        return false;
    }
}

// node: save the file in a directory
// browser: you can assign the file to an audio element
async function saveAudioFileNode(audioData, filename = "result.mp3") {
    try {
        await fs.writeFile(filename, Buffer.from(audioData));
        console.log(`Audio saved to: ${filename}`);
        return true;
    } catch (error) {
        console.error(`Error saving file: ${error.message}`);
        return false;
    }
}

async function demo() {
    const apiKey = "your-openai-api-key-here";
    const audioData = await textToSpeech("Now presenting, a formula one race between hamsters and wombats!", "alloy", apiKey);

    if (audioData) {
        saveAudioFileNode(audioData)
    }
}

demo().catch(console.error);
```

<br><br><br>

## Links

Let op het verschil tussen het gebruiken van een SDK (zoals OpenAI SDK en Azure SDK) en het aanroepen van een REST API met `fetch`. 

- [OpenAI docs](https://platform.openai.com/docs/api-reference/chat/create?lang=node.js)
- [Azure REST API](https://learn.microsoft.com/en-us/rest/api/azure/)
- [Elevenlabs API](https://elevenlabs.io/docs/api-reference/text-to-speech/convert)

