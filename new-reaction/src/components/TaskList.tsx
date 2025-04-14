import { useState } from "react";

interface TaskListProps {
  tasks: string[];
  onSelectTask: (task: string) => void;
}

function TaskList({ tasks, onSelectTask }: TaskListProps) {
  const [selectedIndex, setSelectedIndex] = useState(-1);

  return (
    <>
      <h1>Tasks</h1>
      {tasks.length === 0 && <p>No tasks found</p>}
      <ul className="list-group">
        {tasks.map((task, index) => (
          <li
            className={
              selectedIndex === index
                ? "list-group-item active"
                : "list-group-item"
            }
            key={task}
            onClick={() => {
              setSelectedIndex(index);
              onSelectTask(task);
            }}
          >
            {task}
          </li>
        ))}
      </ul>
    </>
  );
}

export default TaskList;
