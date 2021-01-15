using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessServer.Controllers
{
    public class ChatHub : Hub
    {
        private static List<Item> Items = new List<Item>();

        private static List<string> ChessGroups = new List<string>();
        public async Task<IEnumerable<User>> Login(User user)
        {
            //从在线集合中去除此人
            Items.RemoveAll(o => o.user.UserId == user.UserId);
            Items.Add(new Item()
            {
                Id = Context.ConnectionId,
                user = user
            });

            return Items.Select(o => o.user);
        }

        public async Task<IEnumerable<User>> RequestList()
        {
            return Items.Select(o => o.user);
        }

        public async Task<string> GameRequest(string source, string target)
        {
            //从在线集合中去除此人
            var temp = Items.FirstOrDefault(o => o.user.UserId == source);
            if (temp == null || temp.user.Status == Status.WORKING)
                return "状态异常";

            var user = Items.FirstOrDefault(o => o.user.UserId == target);
            if (user == null)
            {
                return "对方不在线！";
            }

            if (user.user.Status == Status.WORKING)
            {
                return "对方正在对局中！";
            }

            await Clients.Client(user.Id).SendAsync("GameRequest", source);

            return "SUCCESS";
        }

        public async Task<string> ReceiveRequest(string receiver, string beginner)
        {
            //如果接受则不能接受其他的
            //从在线集合中去除此人
            var temp = Items.FirstOrDefault(o => o.user.UserId == receiver);
            if (temp == null || temp.user.Status == Status.WORKING)
                return "状态异常";

            var user = Items.FirstOrDefault(o => o.user.UserId == beginner);
            if (user == null)
            {
                return "对方不在线！";
            }

            if (user.user.Status == Status.WORKING)
            {
                return "对方正在对局中！";
            }

            temp.user.Status = Status.WORKING;
            user.user.Status = Status.WORKING;

            var groupName = $"{receiver}-{beginner}-{DateTime.Now.ToLongTimeString()}";
            //双方状态正常则开始对局
            await Groups.AddToGroupAsync(temp.Id, groupName);
            await Groups.AddToGroupAsync(user.Id, groupName);
            await Clients.Group(groupName).SendAsync("BeginGame", beginner, receiver);
            ChessGroups.Add(groupName);

            return "SUCCESS";
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Items.RemoveAll(o => o.Id == Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task<string> Action(Point oldPoint, Point newPoint, string receiver)
        {
            var temp = Items.FirstOrDefault(o => o.user.UserId == receiver);
            if (temp == null || temp.user.Status != Status.WORKING)
                return "状态异常";

            await Clients.Client(temp.Id).SendAsync("ReceiveLocation", oldPoint, newPoint);
            return "SUCCESS";
        }

        public async Task GameOver()
        {
            var temp = Items.FirstOrDefault(o => o.Id == Context.ConnectionId);
            if (temp == null || temp.user.Status == Status.FREE)
                return;

            temp.user.Status = Status.FREE;

            foreach (var group in ChessGroups)
            {
                if (group.Contains(Context.ConnectionId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                }
            }

            ChessGroups.RemoveAll(o => o.Contains(temp.Id));
        }

        public async Task Exit(string receiver) 
        {
            var temp = Items.FirstOrDefault(o => o.Id == Context.ConnectionId);
            if (temp == null || temp.user.Status == Status.FREE)
                return;

            var target = Items.FirstOrDefault(o => o.user.UserId == receiver);
            if (target == null || target.user.Status == Status.FREE)
                return;

            temp.user.Status = Status.FREE;
            target.user.Status = Status.FREE;

            foreach (var group in ChessGroups)
            {
                if (group.Contains(Context.ConnectionId)|| group.Contains(target.Id))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                    await Groups.RemoveFromGroupAsync(target.Id, group);
                }
            }

            await Clients.Client(target.Id).SendAsync("OtherExit");
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
