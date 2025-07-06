import { useEffect, useState } from "react";
import {
  fetchSets,
  createSet,
  deleteSet,
  updateSet,
  SetDto,
} from "../api/sets";
import Modal from "../components/Modal";

export default function SetsPage() {
  const [sets, setSets] = useState<SetDto[]>([]);
  const [showModal, setShowModal] = useState(false);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [setToDelete, setSetToDelete] = useState<SetDto | null>(null);
  const [setToEdit, setSetToEdit] = useState<SetDto | null>(null);
  const [editTitle, setEditTitle] = useState("");
  const [editDescription, setEditDescription] = useState("");

  useEffect(() => {
    fetchSets()
      .then(setSets)
      .catch(() => alert("Failed to load sets"));
  }, []);

  async function handleCreateSet() {
    try {
      await createSet({ title, description });
      setShowModal(false);
      setTitle("");
      setDescription("");
      const updated = await fetchSets();
      setSets(updated);
    } catch {
      alert("Failed to create set");
    }
  }

  async function handleUpdateSet() {
    if (!setToEdit) return;

    try {
      await updateSet(setToEdit.id, {
        title: editTitle,
        description: editDescription,
      });

      const updated = await fetchSets();
      setSets(updated);
      setSetToEdit(null);
    } catch {
      alert("Failed to update set");
    }
  }

  return (
    <div className="p-6 bg-gray-900 text-white min-h-screen">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Your sets</h1>
        <button
          onClick={() => setShowModal(true)}
          className="bg-blue-600 hover:bg-blue-700 px-4 py-2 rounded text-white"
        >
          + Add new set
        </button>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {sets.map((set) => (
          <div
            key={set.id}
            className="relative p-4 bg-gray-800 rounded shadow hover:shadow-lg transition"
          >
            {/* Кнопка удаления */}
            <button
              onClick={() => setSetToDelete(set)}
              className="absolute top-2 right-2 text-red-400 hover:text-red-600"
              title="Delete set"
            >
              ✕
            </button>

            {/* Кнопка редактирования */}
            <button
              onClick={() => {
                setSetToEdit(set);
                setEditTitle(set.title);
                setEditDescription(set.description);
              }}
              className="absolute top-10 right-1 text-red-400 hover:text-red-600"
              title="Edit set"
            >
              ✏️
            </button>

            <h2 className="text-lg font-bold text-blue-400">{set.title}</h2>
            <p className="text-gray-300 mt-2">{set.description}</p>
            <p className="text-sm text-gray-500">
              Created: {new Date(set.createdAt).toLocaleDateString()}
            </p>
            <p className="text-sm text-gray-500">Words: {set.wordCount}</p>
          </div>
        ))}
      </div>

      {showModal && (
        <Modal
          title="Create New Set"
          onClose={() => setShowModal(false)}
          footer={
            <>
              <button
                onClick={() => setShowModal(false)}
                className="px-4 py-2 bg-gray-600 rounded hover:bg-gray-500"
              >
                Cancel
              </button>
              <button
                onClick={handleCreateSet}
                className="px-4 py-2 bg-blue-600 rounded hover:bg-blue-700"
              >
                Create
              </button>
            </>
          }
        >
          <input
            type="text"
            placeholder="Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="w-full mb-3 px-3 py-2 bg-gray-700 border border-gray-600 rounded"
          />
          <textarea
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full mb-4 px-3 py-2 bg-gray-700 border border-gray-600 rounded resize-none"
          />
        </Modal>
      )}

      {setToDelete && (
        <Modal
          title="Delete Set"
          onClose={() => setSetToDelete(null)}
          footer={
            <>
              <button
                onClick={() => setSetToDelete(null)}
                className="px-4 py-2 bg-gray-600 rounded hover:bg-gray-500"
              >
                Cancel
              </button>
              <button
                onClick={async () => {
                  try {
                    await deleteSet(setToDelete.id);
                    setSets(sets.filter((s) => s.id !== setToDelete.id));
                    setSetToDelete(null);
                  } catch {
                    alert("Failed to delete set");
                  }
                }}
                className="px-4 py-2 bg-red-600 rounded hover:bg-red-700"
              >
                Delete
              </button>
            </>
          }
        >
          <p>
            Are you sure you want to delete <strong>{setToDelete.title}</strong>
            ?
          </p>
        </Modal>
      )}

      {setToEdit && (
        <Modal
          title="Edit set"
          onClose={() => setSetToEdit(null)}
          footer={
            <>
              <button
                onClick={() => setSetToEdit(null)}
                className="px-4 py-2 bg-gray-600 rounded hover:bg-gray-500"
              >
                Cancel
              </button>
              <button
                onClick={handleUpdateSet}
                className="px-4 py-2 bg-green-600 rounded hover:bg-green-700"
              >
                Save
              </button>
            </>
          }
        >
          <input
            type="text"
            placeholder="Title"
            value={editTitle}
            onChange={(e) => setEditTitle(e.target.value)}
            className="w-full mb-3 px-3 py-2 bg-gray-700 border border-gray-600 rounded"
          />
          <textarea
            placeholder="Description"
            value={editDescription}
            onChange={(e) => setEditDescription(e.target.value)}
            className="w-full mb-4 px-3 py-2 bg-gray-700 border border-gray-600 rounded resize-none"
          />
        </Modal>
      )}
    </div>
  );
}
