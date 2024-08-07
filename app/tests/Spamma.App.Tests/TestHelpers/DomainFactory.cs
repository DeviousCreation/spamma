﻿using MaybeMonad;
using Moq;
using ResultMonad;
using Spamma.App.Infrastructure.Contracts.Database;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Tests.TestHelpers;

public static class DomainFactory
    {
        internal static Mock<IUnitOfWork> CreateSuccessfulUnitOfWork()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ResultWithError.Ok<IPersistenceError>);

            return unitOfWork;
        }

        internal static Mock<IUnitOfWork> CreateFailedUnitOfWork()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ResultWithError.Fail<IPersistenceError>(new UniquePersistenceError()));

            return unitOfWork;
        }

        internal static Mock<IRepository<TEntity>> NoData<TEntity>()
            where TEntity : class, IAggregateRoot
        {
            var repository = new Mock<IRepository<TEntity>>();
            repository.Setup(x => x.FindOne(
                    It.IsAny<Specification<TEntity>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(() => Maybe<TEntity>.Nothing);

            return repository;
        }

        internal static Mock<IRepository<TEntity>> Empty<TEntity>(
            Mock<IUnitOfWork> unitOfWork,
            Action<TEntity>? addCallback = null)
            where TEntity : class, IAggregateRoot
        {
            var repository = new Mock<IRepository<TEntity>>();
            repository.Setup(x => x.UnitOfWork)
                .Returns(unitOfWork.Object);
            if (addCallback != null)
            {
                repository.Setup(x => x.Add(It.IsAny<TEntity>()))
                    .Callback(addCallback);
            }

            return repository;
        }

        internal static Mock<IRepository<TEntity>> FindOne<TEntity>(Mock<TEntity> entity)
            where TEntity : class, IAggregateRoot
        {
            var repository = new Mock<IRepository<TEntity>>();
            repository.Setup(x => x.FindOne(
                    It.IsAny<Specification<TEntity>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(() => Maybe.From(entity.Object));

            return repository;
        }

        internal static Mock<IRepository<TEntity>> FindOne<TEntity>(
            Mock<TEntity> entity,
            Mock<IUnitOfWork> unitOfWork)
            where TEntity : class, IAggregateRoot
        {
            var repository = new Mock<IRepository<TEntity>>();
            repository.Setup(x => x.FindOne(
                    It.IsAny<Specification<TEntity>>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(() => Maybe.From(entity.Object));
            repository.Setup(x => x.UnitOfWork)
                .Returns(unitOfWork.Object);

            return repository;
        }
    }