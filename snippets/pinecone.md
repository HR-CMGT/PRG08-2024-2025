# Pinecone

Met Pinecone kan je een cloud vectordatabase aanmaken. Je kan dan rechtstreeks requests aan de cloud database sturen.
Je hebt een API_KEY van pinecone nodig, en de naam van je database. Dit plaats je in de env file:

```sh
npm install @langchain/pinecone
npm install @pinecone-database/pinecone
```

<br>

#### .env

```sh
PINECONE_ENVIRONMENT=
PINECONE_API_KEY=
PINECONE_INDEX=
```

<br><br><bR>

## Vectordata aanmaken

Het inlezen en splitten van tekst is hetzelfde als bij de MemoryVectorStore en FAISS VectorStore.

```js
import { AzureChatOpenAI, AzureOpenAIEmbeddings } from "@langchain/openai";
import { TextLoader } from "langchain/document_loaders/fs/text";
import { RecursiveCharacterTextSplitter } from "langchain/text_splitter";
import { PineconeStore } from "@langchain/pinecone";
import { Pinecone as PineconeClient } from "@pinecone-database/pinecone";

const embeddings = new AzureOpenAIEmbeddings({
    temperature: 0,
    azureOpenAIApiEmbeddingsDeploymentName: process.env.AZURE_EMBEDDING_DEPLOYMENT_NAME
});

const model = new AzureChatOpenAI({ temperature: 1 });
const pinecone = new PineconeClient();
const pineconeIndex = pinecone.Index(process.env.PINECONE_INDEX);

let vectorStore

async function loadTextFile() {
    const loader = new TextLoader("./public/prg8.txt");
    const docs = await loader.load();
    const textSplitter = new RecursiveCharacterTextSplitter({ chunkSize: 500, chunkOverlap: 50 });
    const splitDocs = await textSplitter.splitDocuments(docs);

    // delete the old pinecone index
    await pineconeIndex.deleteAll()
    console.log("🚨 old pinecone index deleted...")

    // create new pinecone index
    vectorStore = await PineconeStore.fromDocuments(splitDocs, embeddings, { pineconeIndex, maxConcurrency: 5 })
    console.log("✅ pinecone store ready")
}

// TEST: alvast een vraag stellen om te testen of de vectorstore werkt
async function askQuestion(prompt) {
    const relevantDocs = await vectorStore.similaritySearch(prompt, 3)
    const context = relevantDocs.map(doc => doc.pageContent).join("\n\n")
    const response = await model.invoke([
        ["system", "You will get a context and a question. Use only the context to answer the question."],
        ["user", `The context is ${context}, the question is ${prompt}`]
    ])
    console.log(response.content)
}

await loadTextFile()
await askQuestion("Waar gaat dit vak over?")
```
<br><br><bR>

## Vragen stellen

Als de pinecone vectorstore live staat kan je requests sturen vanuit je app. Je krijgt dan de meest relevante data terug als tekst, dit stuur je vervolgens weer naar je taalmodel.

```js
import { AzureChatOpenAI, AzureOpenAIEmbeddings } from "@langchain/openai";
import { PineconeStore } from "@langchain/pinecone";
import { Pinecone as PineconeClient } from "@pinecone-database/pinecone";

const embeddings = new AzureOpenAIEmbeddings({
    temperature: 0,
    azureOpenAIApiEmbeddingsDeploymentName: process.env.AZURE_EMBEDDING_DEPLOYMENT_NAME
});

const model = new AzureChatOpenAI({ temperature: 1 });

const pinecone = new PineconeClient();
const pineconeIndex = pinecone.Index(process.env.PINECONE_INDEX);
const vectorStore = await PineconeStore.fromExistingIndex(embeddings, {pineconeIndex, maxConcurrency: 5 });

async function askQuestion(prompt) {
    const relevantDocs = await vectorStore.similaritySearch(prompt, 3)
    const context = relevantDocs.map(doc => doc.pageContent).join("\n\n")
    const response = await model.invoke([
        ["system", "You will get a context and a question. Use only the context to answer the question."],
        ["user", `The context is ${context}, the question is ${prompt}`]
    ])
    console.log(response.content)
}

await askQuestion("Hoeveel studiepunten is dit vak?")
```
