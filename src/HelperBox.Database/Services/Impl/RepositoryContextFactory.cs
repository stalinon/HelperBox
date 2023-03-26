using Microsoft.Extensions.DependencyInjection;

namespace HelperBox.Database.Services.Impl;

/// <inheritdoc cref="IRepositoryContextFactory" />
internal sealed class RepositoryContextFactory : IRepositoryContextFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <inheritdoc cref="RepositoryContextFactory" />
    public RepositoryContextFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <inheritdoc />
    public IRepositoryContext CreateScope() =>
        _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRepositoryContext>();
}
