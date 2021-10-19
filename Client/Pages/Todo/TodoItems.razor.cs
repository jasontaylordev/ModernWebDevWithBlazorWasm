using BlazorDemo.Client.Shared;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorDemo.Client.Pages.Todo
{
    public partial class TodoItems
    {
        [CascadingParameter] public TodoState State { get; set; }

        public TodoItem SelectedItem { get; set; }

        private ElementReference _titleInput;

        private ElementReference _listOptionsModal;

        public bool IsSelectedItem(TodoItem item)
        {
            return SelectedItem == item;

        }

        private async Task AddItem()
        {
            var newItem = new TodoItem { ListId = State.SelectedList.Id };

            State.SelectedList.Items.Add(newItem);

            await EditItem(newItem);
        }

        private async Task ToggleDone(TodoItem item, ChangeEventArgs args)
        {
            if (args != null && args.Value is bool value)
            {
                item.Done = value;

                await State.TodoItemsClient.PutTodoItemAsync(item.Id, item);
            }
        }

        private async Task EditItem(TodoItem item)
        {
            SelectedItem = item;

            await Task.Delay(50);

            if (_titleInput.Context != null)
            {
                await _titleInput.FocusAsync();
            }
        }

        private async Task SaveItem()
        {
            if (SelectedItem.Id == 0)
            {
                if (string.IsNullOrWhiteSpace(SelectedItem.Title))
                {
                    State.SelectedList.Items.Remove(SelectedItem);
                }
                else
                {
                    var item = await State.TodoItemsClient.PostTodoItemAsync(SelectedItem);

                    SelectedItem.Id = item.Id;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SelectedItem.Title))
                {
                    await State.TodoItemsClient.DeleteTodoItemAsync(SelectedItem.Id);
                    State.SelectedList.Items.Remove(SelectedItem);
                }
                else
                {
                    await State.TodoItemsClient.PutTodoItemAsync(SelectedItem.Id, SelectedItem);
                }
            }
        }

        private async Task SaveList()
        {
            await State.TodoListsClient.PutTodoListAsync(State.SelectedList.Id, State.SelectedList);

            State.JS.InvokeVoid(JsInteropConstants.HideModal, _listOptionsModal);

            State.SyncList();
        }

        private async Task DeleteList()
        {
            await State.TodoListsClient.DeleteTodoListAsync(State.SelectedList.Id);

            State.JS.InvokeVoid(JsInteropConstants.HideModal, _listOptionsModal);

            await State.DeleteList();
        }
    }
}
