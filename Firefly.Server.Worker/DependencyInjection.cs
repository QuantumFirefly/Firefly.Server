using Firefly.Server.Core;
using Firefly.Server.Core.Database;
using Firefly.Server.Core.Database.Repositories;
using NLog;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Firefly.Server.Core.Entities.RemoteConfig;
using Firefly.Server.Core.Entities.LocalConfig;
using Firefly.Server.Core.Entities;
using DbConnection = Firefly.Server.Core.Database.DbConnection;
using IDbConnection = Firefly.Server.Core.Database.IDbConnection;
using Microsoft.Extensions.DependencyInjection;
using Firefly.Server.Core.Networking;
using System.Net.Sockets;
using Firefly.Server.Core.Networking.Protocols;
using Firefly.Server.Core.Networking.Protocols.IRC;
using Firefly.Server.Core.Networking.Protocols.IRC.Commands;

namespace Firefly.Server.Worker
{
    internal class DependencyInjection
    {
        internal static IServiceProvider Initalise(IFireflyConfig fireflyConfig, ILogger _log)
        {
            IServiceCollection p = new ServiceCollection()

                    .AddSingleton<IFireflyConfig>(fireflyConfig)
                    .AddSingleton<IGlobalState, GlobalState>()
                    .AddSingleton<ILogger>(_log)

                    .AddScoped<IDbConnection>(p => {
                        return new DbConnection(p.GetRequiredService<IFireflyConfig>().Local?.DbConnectionSettings,
                            p.GetRequiredService<ILogger>());
                    })

                    .AddTransient<IFireflyContext, FireflyContext>()



                    .AddScoped<Func<TcpClient, Guid, IProtocol, IClientConnection>>(p => {
                        return (tcpClient, clientId, protocol) => new ClientConnection(tcpClient,
                                                            clientId,
                                                            p.GetRequiredService<IGlobalState>(),
                                                            p.GetRequiredService<ILogger>(),
                                                            protocol);
                    });

            p = BuildDatabaseDI(p);
            p = BuildIRCDI(p);

            return p.BuildServiceProvider();
        }

        private static IServiceCollection BuildDatabaseDI(IServiceCollection p)
        {
            return p.AddScoped<IUserRepo, UserRepo>()
                .AddScoped<IConfigRepo, ConfigRepo>()

                .AddScoped<IDbContext, DbContext>();
        }

        private static IServiceCollection BuildIRCDI(IServiceCollection p)
        {
            return p.AddScoped<IRCProtocol>()

            .AddSingleton<IRCListener>(p => new IRCListener(
                p.GetRequiredService<IFireflyContext>(),
                p
             ))

            .AddTransient<Func<IRCProtocolState, Func<string, Task>, IEnumerable<IRCCommandBase>>>(sp =>
                (state, sendMessage) => new IRCCommandBase[] {
                    new Nick(sp.GetRequiredService<IFireflyContext>(), sp.GetRequiredService<IDbContext>(), state, sendMessage),
                    new Core.Networking.Protocols.IRC.Commands.User(sp.GetRequiredService<IFireflyContext>(), sp.GetRequiredService<IDbContext>(), state, sendMessage),
                    new Ping(sp.GetRequiredService<IFireflyContext>(), sp.GetRequiredService<IDbContext>(), state, sendMessage),
                    new Cap(sp.GetRequiredService<IFireflyContext>(), sp.GetRequiredService<IDbContext>(), state, sendMessage),
                    new PrivMsg(sp.GetRequiredService<IFireflyContext>(), sp.GetRequiredService<IDbContext>(), state, sendMessage)
                });


        }
    }
}
