# Les 10

## Lokale taalmodellen

Het grote nadeel van werken met online API's is dat het vaak niet gratis is. Je betaalt per token bij de meeste API's. Een ander nadeel is dat je privacy niet is gegarandeerd. Het wordt afgeraden om privé documenten zoals je belastingaangiftes of je afstudeerscriptie te delen met OpenAI, Google Gemini of AliBaba Qwen.

Om die reden kan je kiezen om met een lokaal taalmodel te gaan werken.

<br><br><br>

## Lokaal LLM

Met [OLLama](https://ollama.ai) of [LM Studio](https://lmstudio.ai) kan je LLMs installeren en krijg je meteen een ingebouwde webserver om je prompts naartoe te sturen. Je kan vervolgens [een model kiezen](https://ollama.com/library) om te downloaden. Bijvoorbeeld:

- [Geitje](https://goingdutch.ai/en/posts/introducing-geitje/) (goed in Nederlands)
- [Tolkien](https://huggingface.co/JeremyArancio/llm-tolkien) (goed in fantasy stories)
- [CodeLLama](https://huggingface.co/docs/transformers/en/model_doc/code_llama) (goed in programmeren)

> *🚨 Voor het draaien van een lokaal LLM heb je een recente (4 jaar oud of nieuwer) laptop nodig met minimaal 8GB RAM. Zelfs dan moet je opletten dat je alleen kleine modellen download :***1B of 7B versies***.*

#### Code voorbeeld Ollama

In dit codevoorbeeld roepen we [Ollama aan met langchain](https://js.langchain.com/docs/integrations/chat/ollama/)

```js
import { ChatOllama } from "@langchain/ollama";

const llm = new ChatOllama({
  model: "llama3",
  temperature: 0,
  maxRetries: 2,
});

const aiMsg = await llm.invoke([
  ["system",  "You are a helpful assistant that translates English to Japanese. Translate the user sentence.",],
  ["human", "I love cheesecake."],
]);
```

In dit codevoorbeeld roepen we Ollama aan met de [openai library](https://platform.openai.com/docs/quickstart). Ondanks dat je deze library gebruikt wordt er geen data naar openai verstuurd.

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

<br><br><br>

## Documenten bevragen

Nu je een lokaal taalmodel hebt draaien kan je ook RAG (documenten bevragen) gaan uitvoeren. Het grote voordeel is dat je geen privé documenten naar een server hoeft te sturen. Daarnaast verbrand je geen API tokens. Je kan RAG op twee manieren doen:

- Gebruik dezelfde javascript code als bij het bouwen van embeddings [in les 8 over documenten](https://github.com/HR-CMGT/PRG08-2024-2025/tree/main/les8). Echter nu roep je een lokaal `embedding` model aan dat in OLLama is gedownload. Je moet de embeddings zelf opslaan.
- Download een interface tool die dit voor je kan doen: Je kan kiezen uit [AnythingLLM](https://anythingllm.com) en [OpenWebUI](https://openwebui.com). Het voordeel van deze tools is dat je een drag&drop interface krijgt. Ook slaan ze automatisch je embeddings op. Deze tools hebben ook een API endpoint zodat je vanuit je eigen javascript app vragen kan stellen.

<br><br><br>

## Links

- [OLLama](https://ollama.ai)
- [Calling OLLama from langchain](https://js.langchain.com/docs/integrations/chat/ollama/)
- [LM Studio](https://lmstudio.ai)
- [Open Web UI](https://openwebui.com)
- [Anything LLM](https://anythingllm.com)

## Advanced

- [Ollama models](https://ollama.com/library)
- [Calling Ollama with OpenAI library](https://platform.openai.com/docs/quickstart))
- [HuggingFace models](https://huggingface.co/models?other=text-generation)
- [Langchain embedding met OLLama](https://js.langchain.com/docs/use_cases/question_answering/local_retrieval_qa).
- [Download LLMs met python en OpenLLM](https://github.com/bentoml/OpenLLM), inclusief webserver en [Langchain integratie](https://python.langchain.com/docs/integrations/llms/openllm)
- [HuggingFace tutorial](https://www.markhneedham.com/blog/2023/06/23/hugging-face-run-llm-model-locally-laptop/) en [6 ways to run a local LLM on your laptop](https://semaphoreci.com/blog/local-llm).
- [Huggingface models in Langchain](https://python.langchain.com/docs/integrations/llms/huggingface_pipelines)
- [Running your own private Copilot](https://www.youtube.com/watch?v=F1bXfnrzAxM)
- [Huggingface endpoints](https://huggingface.co/blog/inference-endpoints-llm) 💰.
- [Geitje endpoint](./huggingface.md).
