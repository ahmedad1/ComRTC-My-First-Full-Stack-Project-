IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] varchar(600) NOT NULL,
        [Name] varchar(600) NULL,
        [NormalizedName] varchar(600) NULL,
        [ConcurrencyStamp] varchar(600) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] varchar(600) NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [UserName] varchar(600) NULL,
        [NormalizedUserName] varchar(600) NULL,
        [Email] varchar(600) NULL,
        [NormalizedEmail] varchar(600) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] varchar(600) NULL,
        [SecurityStamp] varchar(600) NULL,
        [ConcurrencyStamp] varchar(600) NULL,
        [PhoneNumber] varchar(600) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [BlackListJwts] (
        [TokenString] varchar(600) NOT NULL,
        CONSTRAINT [PK_BlackListJwts] PRIMARY KEY ([TokenString])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [Groups] (
        [Id] int NOT NULL IDENTITY,
        [GroupName] varchar(600) NOT NULL,
        CONSTRAINT [PK_Groups] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Product_Name] varchar(600) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] varchar(600) NOT NULL,
        [ClaimType] varchar(600) NULL,
        [ClaimValue] varchar(600) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] varchar(600) NOT NULL,
        [ClaimType] varchar(600) NULL,
        [ClaimValue] varchar(600) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] varchar(600) NOT NULL,
        [ProviderKey] varchar(600) NOT NULL,
        [ProviderDisplayName] varchar(600) NULL,
        [UserId] varchar(600) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] varchar(600) NOT NULL,
        [RoleId] varchar(600) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] varchar(600) NOT NULL,
        [LoginProvider] varchar(600) NOT NULL,
        [Name] varchar(600) NOT NULL,
        [Value] varchar(600) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [IdentityToken] (
        [UserId] varchar(600) NOT NULL,
        [Token] nvarchar(44) NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        CONSTRAINT [PK_IdentityToken] PRIMARY KEY ([UserId], [Token]),
        CONSTRAINT [FK_IdentityToken_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [RefreshTokens] (
        [Id] varchar(600) NOT NULL,
        [Token] varchar(600) NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [ExpiresOn] datetime2 NOT NULL,
        [RevokedOn] datetime2 NULL,
        [UserId] varchar(600) NOT NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [UserConnections] (
        [ConnectionId] varchar(600) NOT NULL,
        [UserId] varchar(600) NOT NULL,
        CONSTRAINT [PK_UserConnections] PRIMARY KEY ([ConnectionId]),
        CONSTRAINT [FK_UserConnections_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [Verifications] (
        [Id] varchar(600) NOT NULL,
        [UserId] varchar(600) NOT NULL,
        [Code] varchar(600) NOT NULL,
        [ExpiresOn] datetime2 NOT NULL,
        CONSTRAINT [PK_Verifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Verifications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE TABLE [UserGroups] (
        [UserId] varchar(600) NOT NULL,
        [GroupId] int NOT NULL,
        CONSTRAINT [PK_UserGroups] PRIMARY KEY ([GroupId], [UserId]),
        CONSTRAINT [FK_UserGroups_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserGroups_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (''a5aecd95-9d3d-4527-9dbe-1b7d45604753'', ''fc1b8338-8a9b-469e-aa96-b9ebc00475f6'', ''user'', ''USER'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_IdentityToken_UserId] ON [IdentityToken] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [IX_UserConnections_UserId] ON [UserConnections] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE INDEX [IX_UserGroups_UserId] ON [UserGroups] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Verifications_UserId] ON [Verifications] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240605122852_Mig'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240605122852_Mig', N'8.0.6');
END;
GO

COMMIT;
GO

