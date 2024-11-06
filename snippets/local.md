# Models

In de [online community](https://huggingface.co/models?other=text-generation) verschijnen dagelijks nieuwe models. Om hiermee te werken zal je een LLM moeten downloaden, of hosten. Een model kan een specifiek doel hebben, bv:
- [Geitje](https://goingdutch.ai/en/posts/introducing-geitje/) (goed in Nederlands)
- [Tolkien](https://huggingface.co/JeremyArancio/llm-tolkien) (goed in fantasy stories)
- [CodeLLama](https://huggingface.co/docs/transformers/en/model_doc/code_llama) (goed in programmeren)

<br>

## Lokaal LLM

Als je zeker wil weten dat je data privé blijft, op je eigen machine, dan kan je een open source taalmodel installeren. Je betaalt nu ook geen tokens om taaldata te genereren. 

Met [OLLama](https://ollama.ai) of [LM Studio](https://lmstudio.ai) kan je LLMs installeren en krijg je meteen een ingebouwde webserver om je prompts naartoe te sturen.

> *🚨 Voor het draaien van een lokaal LLM heb je een krachtige laptop nodig. Zelfs dan moet je opletten dat je alleen kleine modellen download :***1B of 7B versies***.*

#### Code voorbeeld LM Studio / Ollama

In dit codevoorbeeld wordt het lokale LLM aangeroepen met de standaard `openai` api.

```js
import { OpenAI } from "openai";

const openai = new OpenAI({
    temperature: 0.3,
    apiKey: "not-needed",
    baseURL: "http://localhost:1234/v1",
});

async function main() {
    const completion = await openai.chat.completions.create({
        messages: [
            { role: "system", content: "You are a helpful assistant." },
            { role : "user",  content: "Hello, can you tell me a joke."}
        ],
    });

    console.log(completion.choices[0]);
}

main();
```

<br>

## HuggingFace Spaces

- *HuggingFace Spaces* biedt hosting met automatische javascript endpoints voor [LLM models](https://huggingface.co/blog/inference-endpoints-llm) 💰.
- Spaces van andere users bieden soms een gratis endpoint aan. Zie dit voorbeeld voor een [Geitje endpoint](./huggingface.md).

<br>

## Links

- [OLLama](https://ollama.ai) 
- [LM Studio](https://lmstudio.ai)
- [HuggingFace models](https://huggingface.co/models?other=text-generation)
- [HuggingFace hosting and creating javascript endpoints](https://ui.endpoints.huggingface.co/welcome)
- [Langchain embedding met OLLama](https://js.langchain.com/docs/use_cases/question_answering/local_retrieval_qa).
- [Download LLMs met python en OpenLLM](https://github.com/bentoml/OpenLLM), inclusief webserver en [Langchain integratie](https://python.langchain.com/docs/integrations/llms/openllm)
- [HuggingFace tutorial](https://www.markhneedham.com/blog/2023/06/23/hugging-face-run-llm-model-locally-laptop/) en [6 ways to run a local LLM on your laptop](https://semaphoreci.com/blog/local-llm).
- [Huggingface models in Langchain](https://python.langchain.com/docs/integrations/llms/huggingface_pipelines)
- [Running your own private Copilot](https://www.youtube.com/watch?v=F1bXfnrzAxM)
