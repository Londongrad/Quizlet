import { useEffect, useState } from "react";
import { fetchSets, SetDto } from "../api/sets";

export default function SetsPage() {
  const [sets, setSets] = useState<SetDto[]>([]);

  useEffect(() => {
    fetchSets()
      .then(setSets)
      .catch(() => alert("Failed to load sets"));
  }, []);

  return (
    <div className="p-6 bg-gray-50 min-h-screen">
      <h1 className="text-3xl font-bold mb-6">Your Sets</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {sets.map((set) => (
          <div
            key={set.id}
            className="p-4 bg-white rounded shadow hover:shadow-lg transition"
          >
            <h2 className="text-lg font-bold text-blue-700">{set.title}</h2>
            <p className="text-gray-600 mt-2">{set.description}</p>
            <p className="text-sm text-gray-500">
              Created: {new Date(set.createdAt).toLocaleDateString()}
            </p>
            <p className="text-sm text-gray-500">Words: {set.wordCount}</p>
          </div>
        ))}
      </div>
    </div>
  );
}
