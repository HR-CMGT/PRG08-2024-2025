# 🫣 HuggingFace 

## Langchain

Huggingface biedt een optie om via je HuggingFace account gratis een beperkt aantal calls naar allerlei taalmodellen te doen. 
Je hoeft dan geen creditcard te hebben. [Overzicht modellen en code voorbeeld](https://huggingface.co/docs/inference-providers/en/index).
 
DeepSeek 3 voorbeeld
 
```js
// npm install @langchain/openai 
import { ChatOpenAI } from "@langchain/openai";

const model = new ChatOpenAI({
    configuration: {
        baseURL: "https://router.huggingface.co/novita/v3/openai",
        apiKey: process.env.HUGGINGFACE_KEY,
    },
    modelName: "deepseek/deepseek-v3-0324",
    maxTokens: 500,
    streaming: false,
});

const chat = await model.invoke("Why do beavers build dams?")
console.log(chat.content)
 ```


## Spaces

[HuggingFace Spaces](https://huggingface.co/spaces) biedt hosting met automatische javascript endpoints voor [LLM models](https://huggingface.co/blog/inference-endpoints-llm) 💰. In de [Geitje Space](https://huggingface.co/spaces/Rijgersberg/GEITje-7B-chat) kan je het Nederlandse taalmodel "geitje" uitproberen. Spaces van HuggingFace users bieden soms ook een javascript endpoint aan. 

<br><Br>

## Transformers.js

Met de HuggingFace `Transformers.js` library kan je taalmodellen rechtstreeks aanroepen via javascript. Er zijn ontzettend veel modellen, niet alle modellen bieden javascript support. Je moet hierbij letten op de "ONNX" versie van een model. Let ook op de taak die je wil uitvoeren (text generation, text classification, etc.). De taak "chat conversation" lijkt op dit moment niet beschikbaar in `transformers.js`.

In deze code snippet wordt het `Xenova/distilbert` model gebruikt om te kijken of een tekst positief of negatief is.

```js
import { pipeline } from 'https://cdn.jsdelivr.net/npm/@xenova/transformers@2.14.2';

// When running for the first time, the pipeline will download and cache the model.
// This can take a while! Models will be stored in browser cache
let classifier = await pipeline('sentiment-analysis');
let result = await classifier('I love programming AI!');
console.log(result[0].label)
```


<br><br><br>

## Links

- [HuggingFace Transformers.js examples](https://huggingface.co/docs/transformers.js/en/pipelines)
- [HuggingFace Text Models with support for transformers.js](https://huggingface.co/models?pipeline_tag=text-generation&library=transformers.js&sort=trending&search=onnx)
