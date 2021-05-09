// <copyright file="IDbConnectionFactory.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System.Data;

namespace Solarponics.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}