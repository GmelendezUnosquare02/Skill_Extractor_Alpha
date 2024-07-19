async function query(data) {
    const response = await fetch(
        "https://api-inference.huggingface.co/models/yanekyuk/bert-keyword-extractor",
        {
            headers: { Authorization: "Bearer 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" },
            method: "POST",
            body: JSON.stringify(data),
        }
    );
    const result = await response.json();
    return result;
}

window.callHuggingFaceAPI = async function (text) {
    const result = await query({ "inputs": text });
    return JSON.stringify(result); // Return the stringified JSON object
}