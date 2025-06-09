// app/feedback/page.tsx
"use client";

import { useState } from "react";

export default function FeedbackPage() {
  const [nome, setNome] = useState("");
  const [comentario, setComentario] = useState("");
  const [nota, setNota] = useState(5);
  const [mensagem, setMensagem] = useState("");

  const enviarFeedback = async () => {
    try {
      const response = await fetch("/api/feedback", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ nome, comentario, nota }),
      });

      if (response.ok) {
        setMensagem("Feedback enviado com sucesso!");
        setNome("");
        setComentario("");
        setNota(5);
      } else {
        setMensagem("Erro ao enviar feedback.");
      }
    } catch (error) {
      setMensagem("Erro ao conectar com o servidor.");
    }
  };

  return (
    <main className="max-w-xl mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Deixe seu Feedback</h1>

      {mensagem && <p className="mb-4 text-sm text-green-600">{mensagem}</p>}

      <div className="flex flex-col gap-4">
        <input
          type="text"
          placeholder="Seu nome"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          className="p-2 border rounded"
        />

        <textarea
          placeholder="Comentário sobre o jogo"
          value={comentario}
          onChange={(e) => setComentario(e.target.value)}
          className="p-2 border rounded"
        />

        <label className="flex flex-col">
          Nota (0 a 10):
          <input
            type="number"
            min={0}
            max={10}
            value={nota}
            onChange={(e) => setNota(parseInt(e.target.value))}
            className="p-2 border rounded w-24 mt-1"
          />
        </label>

        <button
          onClick={enviarFeedback}
          className="bg-blue-600 hover:bg-blue-700 text-white py-2 rounded"
        >
          Enviar
        </button>
      </div>
    </main>
  );
}
