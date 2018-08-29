using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{

    public bool started { get; protected set; }
    public bool completed;

    public Type type;

    public enum Type
    {
        Task,
        Subtask
    }

    protected Task task;

    public IList<Task> subtasks { get; protected set; }

    void Awake()
    {
        started = false;
        completed = false;
        subtasks = new List<Task>();

        type = Type.Task;
        OnAwake();
    }

    public virtual void OnAwake(){}

    void Start()
    {
        OnStart();
    }

    public virtual void OnStart(){}

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate(){}

    public void addSubTask(Task substask)
    {
        subtasks.Add(substask);
        substask.OnTaskStart();
        TasksManager.GetInstance().OnTaskUpdated();
    }

    public string description { get; protected set; }

    public virtual void OnTaskStart()
    {
        if (type == Type.Task)
        {
            TasksManager.GetInstance().SetTask(this);
        }
        started = true;
    }
    public virtual void OnTaskFinish()
    {
        TasksManager.GetInstance().FinishTask(this);
    }


}
