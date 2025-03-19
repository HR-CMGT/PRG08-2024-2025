# Les 8

## Eigen documenten lezen met een taalmodel

In deze oefening gaan we vragen over een document beantwoorden met een taalmodel. Je werkt in drie losse projecten:

- Voorbereiding: tekst inladen, vectordata maken, opslaan
- Vectordata inladen en vragen beantwoorden

<br>


> ⚠️ Updated naar langchain 0.3. Zorg dat je de laatste langchain versie hebt:
```sh
npm install @langchain/openai @langchain/community @langchain/core @langchain/textsplitter faiss-node
```

<br>
<br>
<br>

### Inhoud

- Tekst omzetten naar vectoren
- Tekstbestand inlezen
- Vragen beantwoorden
- Vector stores
- Chat history
- Privacy en copyright

<br><br><br>

## Tekst omzetten naar vectoren

Taalmodellen werken met `vectordata` om verbanden tussen teksten te kunnen leggen. Jouw tekst moet je dus omzetten naar vectordata. Dit noemen we `embedding`. Om tekst te `embedden` heb je een taalmodel nodig dat *geen* volledig chat model is. OpenAI gebruikt hier het `ada` model voor:

```js
import { OpenAIEmbeddings, ChatOpenAI } from "@langchain/openai"

// om te chatten
const model = new ChatOpenAI({
    temperature: 0.3,
    azureOpenAIApiKey: process.env.AZURE_OPENAI_API_KEY,
    azureOpenAIApiVersion: process.env.OPENAI_API_VERSION,
    azureOpenAIApiInstanceName: process.env.INSTANCE_NAME,
    azureOpenAIApiDeploymentName: process.env.ENGINE_NAME, // chatgpt3.5
})

// om embeddings te maken
const embeddings = new OpenAIEmbeddings({
    temperature: 0.1,
    azureOpenAIApiKey: process.env.AZURE_OPENAI_API_KEY,
    azureOpenAIApiVersion: process.env.OPENAI_API_VERSION,
    azureOpenAIApiInstanceName: process.env.INSTANCE_NAME,
    azureOpenAIApiDeploymentName: process.env.DEPLOYMENT_NAME,  // ada
})
```
#### Hello world test

```js
const vectordata = await embeddings.embedQuery("Hello world")
console.log(vectordata)
console.log(`Created vector with ${vectordata.length} values.`)
```
[Langchain documentatie voor Azure OpenAI embedding](https://js.langchain.com/docs/integrations/text_embedding/azure_openai).

<br><br><Br>

## Tekstbestand inlezen

Langchain heeft verschillende opties om tekstbestanden te lezen, zoals [.txt, PDF, JSON, CSV, etc.](https://js.langchain.com/docs/modules/data_connection/document_loaders/). Je kan zelfs een [hele github repository](https://js.langchain.com/docs/integrations/document_loaders/web_loaders/github#usage) inlezen. In dit voorbeeld lezen we een `.txt` file. De ingelezen tekst 'knip' je op in chunks. Doordat de chuncks een vaste grootte hebben kan het gebeuren dat je midden in een zin/passage knipt. Om te voorkomen dat je chunks met halve informatie krijgt geef je de chunks een overlap. 

```js
import { TextLoader } from "langchain/document_loaders/fs/text";
import { RecursiveCharacterTextSplitter } from "langchain/text_splitter";
import { MemoryVectorStore } from "langchain/vectorstores/memory";

let vectorStore

async function createVectorstore() {
    const loader = new TextLoader("./public/example.txt");
    const docs = await loader.load();
    const textSplitter = new RecursiveCharacterTextSplitter({ chunkSize: 1000, chunkOverlap: 200 });
    const splitDocs = await textSplitter.splitDocuments(docs);
    console.log(`Document split into ${splitDocs.length} chunks. Now saving into vector store`);
    vectorStore = await MemoryVectorStore.fromDocuments(splitDocs, embeddings);
}
```
<br><br><br>

## Vragen stellen over de tekst

In bovenstaand voorbeeld slaan we de vectordata op in een `MemoryVectorStore`. Hier kan je vragen aan stellen. Dit werkt als volgt:

- Je geeft een prompt.
- Langchain zoekt in je vectorstore naar tekst die daar het beste bij past.
- Het prompt en het zoekresultaat worden naar OpenAI gestuurd om er een mooi antwoord van te maken.

```js
async function askQuestion(){
    const relevantDocs = await vectorStore.similaritySearch("What is this document about?",3);
    const context = relevantDocs.map(doc => doc.pageContent).join("\n\n");
    console.log("Asking the model...");
    const response = await model.invoke([
        ["system", "Use the following context to answer the user's question. Only use information from the context."],
        ["user", `Context: ${context}\n\nQuestion: What is this document about?`]
    ]);
    console.log("\nAnswer found:");
    console.log(response.content);
}
```

<br><br><br>

## Vectorstore bewaren

De `MemoryVectorStore` blijft niet bewaard tussen sessies. Daardoor moet je de embeddings telkens opnieuw maken, dat kost tijd en tokens. Om dat te voorkomen kan je de embeddings opslaan in een vectorstore:

- *Lokaal bestand*. De vectordata wordt als lokaal bestand binnen je project opgeslagen, bv. met FAISS.
- *Vector database*. Dit is een "echte" database op je systeem, zoals ChromaDB, MongoDB.
- *Cloud database*. Je vectordata staat in de cloud, waardoor het vanuit meerdere projecten online toegankelijk is. Sommige cloud services, zoals [pinecone](https://www.pinecone.io) bieden 1 gratis online vectorstore.

In dit codevoorbeeld slaan we de vectorstore op met FAISS. 

#### data opslaan
```js
import { FaissStore } from "@langchain/community/vectorstores/faiss";

vectorStore = await FaissStore.fromDocuments(splitDocs, embeddings);
await vectorStore.save("vectordatabase"); // geef hier de naam van de directory waar je de data gaat opslaan
```
#### data inlezen
```js
vectorStore = await FaissStore.load("vectordatabase", embeddings); // dezelfde naam van de directory 
```
> ⚠️ ***Let op dat je de data maar 1x hoeft op te slaan.*** Om die reden is het handiger om met twee losse `.js` files te werken. Een voor het maken van vectordata, en een voor het inlezen en vragen stellen.

<br><br><br>

### Chat History

In bovenstaand code voorbeeld stuur je een array naar het taalmodel om een antwoord te krijgen. Als je dit antwoord en opvolgende vragen telkens aan deze array toevoegt ontstaat een chat history. Dit helpt het model om de conversatie te kunnen volgen.

```js
let history = [
    ["system", "Use the following context to answer the user's question. Only use information from the context."],
    ["user", `Context: ${context}\n\nQuestion: What is this document about?`]
]
const response = await model.invoke(history);

// antwoord toevoegen aan chat history
history.push(["ai", response.content])
```
#### Expert level

[Gebruik langchain classes voor het automatisch bijhouden van chat history](https://js.langchain.com/docs/how_to/qa_chat_history_how_to/)

<br><br><br>


## Server

Op je `express` server *(zie [les 6](../les6/README.md))* voeg je nu de FAISS data toe en de code waarmee je vragen aan het document kan stellen. Het genereren van de tekstdata hoeft niet persé op de server te staan.

#### React integratie

Omdat alle "document code" op de server plaatsvindt hoef je in React je prompt code niet te veranderen.


<br><br><br>


## Troubleshooting

Let op dat je de laatste versie van langchain en faiss hebt geinstalleerd.

```sh
npm install @langchain/openai @langchain/community @langchain/core @langchain/textsplitter faiss-node
```


<br><br><br>

## Privacy en copyright

Bij het maken van embeddings verstuur je een document naar OpenAI en/of Microsoft. Het is daarom belangrijk dat je geen data verstuurt die auteursrechtelijk beschermd is. Om data helemaal veilig te houden zou je kunnen werken met een [Lokaal LLM of je eigen LLM hosting](../snippets/local.md)


<br>
<br>
<br>


## Voorbeeld

Bovenstaande code is gebruikt voor de [PRG4 assistent van jaar 1](https://ai-assistent-mu.vercel.app)

<br><br><br>

 ## Links

- [Langchain Azure OpenAI Text Embedding](https://js.langchain.com/docs/integrations/text_embedding/azure_openai)
- [Langchain document loaders](https://js.langchain.com/docs/modules/data_connection/document_loaders/)
- [FAISS - Facebook Vector Store](https://js.langchain.com/docs/integrations/vectorstores/faiss)
