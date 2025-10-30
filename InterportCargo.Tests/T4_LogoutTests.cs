using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "T4")]
    [Collection("E2E-Sqlite")]
    public class T4_LogoutTests
    {
        [Fact]
        public void Logout_Clears_Session_Status()
        {
            var page = new InterportCargo.Pages.Account.LogoutModel();

            var http = new DefaultHttpContext();
            http.Features.Set<ISessionFeature>(new TestSessionFeature());
            http.Session.SetString("IsAuthenticated", "true");
            http.Session.SetString("UserType", "Customer");

            var actionContext = new ActionContext(http, new RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            page.PageContext = new PageContext(actionContext);

            var result = page.OnGet();

            Assert.IsType<PageResult>(result);
            Assert.False(http.Session.TryGetValue("IsAuthenticated", out _));
            Assert.False(http.Session.TryGetValue("UserType", out _));
        }
        private sealed class TestSessionFeature : ISessionFeature
        {
            public ISession Session { get; set; } = new TestSession();
        }
        private sealed class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _store = new();
            public IEnumerable<string> Keys => _store.Keys;
            public string Id { get; } = Guid.NewGuid().ToString();
            public bool IsAvailable => true;
            public void Clear() => _store.Clear();
            public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public void Remove(string key) => _store.Remove(key);
            public void Set(string key, byte[] value) => _store[key] = value;
            public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value!);
        }
    }
}
