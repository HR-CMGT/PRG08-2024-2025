# Les 8

## Eigen documenten lezen met een taalmodel

In deze oefening gaan we vragen over een document beantwoorden met een taalmodel. Je werkt in drie losse projecten:

- Voorbereiding: tekst inladen, embedden, model opslaan
- Server: model inladen, vragen beantwoorden met LLM
- Client: vragen sturen vanuit de user interface

<br>

### Inhoud

- Werken met grote hoeveelheden tekst
- Tekst omzetten naar vectoren
- Tekstbestand inlezen
- Vragen beantwoorden
- Vector stores
- Privacy en copyright

<br><br><br>

## Tekst omzetten naar vectoren

In het college heb je gezien waarom teksten als `vectordata` worden gebruikt in taalmodellen. Jouw tekst moet je dus omzetten naar vectordata. Dit noemen we `embedding`. Om tekst te `embedden` heb je een taalmodel nodig dat *geen* volledig chat model is. OpenAI gebruikt hier het `ada` model voor. 

We gebruiken het `embedding` (ada) model om vectordata te maken. We hebben het `chat` model (chatgpt) uit de vorige les ook nodig om vragen te stellen. De waarden voor de keys staan in de `.env` file.

```js
import { OpenAIEmbeddings, ChatOpenAI } from "@langchain/openai"

const chatmodel = new ChatOpenAI({
    temperature: 0.3,
    azureOpenAIApiKey: process.env...,
    azureOpenAIApiVersion: process.env...,
    azureOpenAIApiInstanceName: process.env...,
    azureOpenAIApiDeploymentName: process.env..., // hier vul je het `chatgpt` model in
})

const embeddings = new OpenAIEmbeddings({
    temperature: 0.1,
    azureOpenAIApiKey: process.env...,
    azureOpenAIApiVersion: process.env...,
    azureOpenAIApiInstanceName: process.env...,
    azureOpenAIApiDeploymentName: process.env...,  // hier vul je het 'ada' model in
})
```
#### Hello world test

```js
const vectordata = await embeddings.embedQuery("Hello world")
console.log(vectordata)
console.log(`Created vector with ${vectordata.length} values.`)
```
[Langchain documentatie voor Azure OpenAI embedding](https://js.langchain.com/docs/integrations/text_embedding/azure_openai).

<br>

## Tekstbestand inlezen

Langchain heeft verschillende opties om tekstbestanden te lezen, zoals [.txt, PDF, JSON, CSV, etc.](https://js.langchain.com/docs/modules/data_connection/document_loaders/). Je kan zelfs een [hele github repository](https://js.langchain.com/docs/integrations/document_loaders/web_loaders/github#usage) inlezen. In dit voorbeeld lezen we een `.txt` file. De ingelezen tekst 'knip' je op in chunks. Doordat de chuncks een vaste grootte hebben kan het gebeuren dat je midden in een zin/passage knipt. Om te voorkomen dat je chunks met halve informatie krijgt geef je de chunks een overlap. De grootte van de chunks en de overlap moet je zelf kiezen.

```js
import { RecursiveCharacterTextSplitter } from "langchain/text_splitter"
import { TextLoader } from "langchain/document_loaders/fs/text"
import { MemoryVectorStore } from "langchain/vectorstores/memory"
import { RetrievalQAChain } from "langchain/chains"

const loader = new TextLoader("./myfile.txt")
const data = await loader.load()
const textSplitter = new RecursiveCharacterTextSplitter({chunkSize: 1500, chunkOverlap: 100})
const splitDocs = await textSplitter.splitDocuments(data)
```
<br><br><br>

## Vectordata maken en vragen stellen
Je gaat de ingeladen tekstdata omzetten naar vectordata. In dit voorbeeld slaan we de vectordata op in een `MemoryVectorStore`. 
```js
const vectorStore = await MemoryVectorStore.fromDocuments(splitDocs, embeddings)
```
🤯 Je kan nu vragen stellen aan je eigen document! 

```js
const chain = RetrievalQAChain.fromLLM(chatmodel, vectorStore.asRetriever())
const response = await chain.call({ query: "who is the text about?" })
console.log(response.text)
```
We hebben nu kunnen testen of we kunnen werken met het opslaan van vectordata en het stellen van vragen. Om deze app af te maken gaan we nog twee dingen toevoegen:

- Automatisch bijhouden van ***chat history***. Dit is optioneel, het zorgt ervoor dat je een conversatie kan voeren waarbij eerdere antwoorden ook meegenomen worden.
- Opslaan van de ***VectorStore*** zodat je niet telkens opnieuw vectordata hoeft aan te maken voordat de chat begint.

<br><br><br>

### Automatic Chat History

In onderstaand voorbeeld wordt de chat history automatisch bijgehouden dankzij de `BufferMemory` class van langchain. Je hoeft nu niet meer zelf een chat history in een array op te slaan.

We geven de `vectorStore` en de `bufferMemory` mee aan langchain als we een vraag stellen.

```js
const memory = new BufferMemory({ memoryKey: "chat_history", returnMessages: true })
const chain = ConversationalRetrievalQAChain.fromLLM(model, vectorStore.asRetriever(), { memory })
const answer = await chain.call({ question: "Waar gaat deze tekst over?" })
console.log(answer)
```

### Achtergrond informatie: de conversatie-ketting

Je hebt nu met relatief weinig code een chatbot waarmee je kunt praten over je document. Dit is wat LangChain voor je doet nadat je een `call` met een `question` doet: 
* De `question` wordt samen met de `chat_history` naar het chatmodel gestuurd met als prompt:  
  'Gebruik deze `chat_history` en vervolgvraag `question` om een op zichzelf staande vraag te maken'
* Het chatmodel stuurt een op zichzelf staande vraag terug.
* Er wordt een call gedaan naar het embeddingsmodel om de op zichzelf staande vraag te embedden.
* Deze embedding wordt gebruikt om relevante teksten voor deze vraag in de vectorstore op te zoeken.
* Daarna wordt de op zichzelf staande vraag, samen met de relevante teksten naar het chatmodel gestuurd met prompt:  
  'Gebruik deze teksten om antwoord te geven op deze vraag'
* Het chatmodel stuurt een anwtoord terug dat komt uit de informatie van jouw documenten.
* Het antwoord wordt samen met jouw `question` toegevoegd aan de `chat_history`.

<br><br><br>

## Vector stores

Het doen van prompts (genereren van tokens) kost geld. Een simpele prompt kost bijna niets, maar het maken van embeddings kan sneller oplopen, als je veel tekst laat embedden. Daarnaast is het zonde om steeds dezelfde embeddings te laten maken, vandaar dat je de wilt kunnen opslaan. Bekijk [hier een lijst van vectorstores](https://js.langchain.com/docs/integrations/vectorstores) die je via langchain kan gebruiken. In het algemeen kan je dit onderscheid maken:

- *Lokaal bestand*. De vectordata wordt als lokaal bestand binnen je project opgeslagen.
- *Vector database*. Dit is een "echte" database op je systeem, zoals ChromaDB, MongoDB.
- *Cloud database*. Je vectordata staat in de cloud, waardoor het vanuit meerdere projecten online toegankelijk is. Sommige cloud services, zoals [pinecone](https://www.pinecone.io) bieden 1 gratis online vectorstore.

<br><br><br>


## FAISS

Voor deze les werken we met [FAISS - Facebook Vector Store](https://js.langchain.com/docs/integrations/vectorstores/faiss). 

#### Aanmaken en opslaan vectordata

Plaats je code voor het aanmaken van vectordata in een eigen `.js` file.

```js
import { FaissStore } from "langchain/vectorstores/faiss"

const loader = new TextLoader("public/lord-of-the-rings.txt")
const data = await loader.load()
const textSplitter = new RecursiveCharacterTextSplitter({ chunkSize: 1000, chunkOverlap: 2 })
const splitDocs = await textSplitter.splitDocuments(data)

// hier maken we de FAISS vectorstore en slaan de data daarin op
const vectorStore = await FaissStore.fromDocuments(splitDocs, embeddings)
const directory = "database"
await vectorStore.save(directory)
console.log("store is saved!")
```

#### Vragen stellen aan vectordata

Plaats je code voor het stellen van vragen in een eigen `.js` file. Hierin kan je nu dezelfde `faiss` store inladen die je in het vorige proces had opgeslagen.

```js
import { FaissStore } from "langchain/vectorstores/faiss"
import { ChatOpenAI } from "@langchain/openai"
import { OpenAIEmbeddings } from "@langchain/openai"
import { ConversationalRetrievalQAChain } from "langchain/chains"
import { BufferMemory } from "langchain/memory"

const KEY = process.env.OPENAI_API_KEY
const embeddings = new OpenAIEmbeddings({ openAIApiKey: KEY })
const directory = "database"
const vectorStore = await FaissStore.load(directory,embeddings)

const model = new ChatOpenAI({ temperature: 0.1, modelName: "gpt-3.5-turbo",  openAIApiKey: KEY })
const memory = new BufferMemory({ memoryKey: "chat_history", returnMessages: true })
const chain = ConversationalRetrievalQAChain.fromLLM(model,vectorStore.asRetriever(),{ memory })

async function answerQuestion(prompt) {
    const result = await chain.call({ question: prompt })
    return result.text
}
```

<br><br><br>


## Server

Op je `express` server *(zie [les 6](../les6/README.md))* voeg je nu de FAISS data toe. Het bestand waarmee je vectordata aanmaakt wordt niet gebruikt op de server. Dit bestand roep je handmatig aan op het moment dat je andere teksten wil kunnen bevragen.

<br><br><br>


## Troubleshooting

Je krijgt een error over `size of k`. OpenAI verwacht minimaal 3 chunks in je tekst. Als er minder zijn kan je dit aangeven. In dit voorbeeld zijn er maar 2 chunks:

```js
const chain = RetrievalQAChain.fromLLM(model, vectorStore.asRetriever({ k: 2 }))
```

De FAISS documentatie is recent veranderd. Je kan deze waarschuwing krijgen. 

> *[WARNING]: Importing from "langchain/vectorstores/faiss" is deprecated. Instead, please add the "@langchain/community" package to your project with e.g. `npm install @langchain/community` en `import from "@langchain/community/vectorstores/faiss"`.*



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
