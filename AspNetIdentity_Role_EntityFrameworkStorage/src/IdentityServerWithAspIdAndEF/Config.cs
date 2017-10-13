// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerWithAspNetIdentity
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API",new[] { JwtClaimTypes.Name, JwtClaimTypes.Role })
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                }
            };
        }

        private static readonly string[] roles = new[] {
        "Admin",
        "User",
        "Auditor"
    };
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {

            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new IdentityRole(role);
                    var create = await roleManager.CreateAsync(newRole);
                    if (!create.Succeeded)
                    {

                        throw new Exception("Failed to create role");
                    }
                    else
                    {
                       await roleManager.AddClaimAsync(newRole, new Claim("lordchinzilla/createdOn", DateTime.Now.ToLongDateString()));
                       await roleManager.AddClaimAsync(newRole, new Claim("lordchinzilla/CorpUsers", "true"));

                    }
                }

            }

        }
        public static async Task ClearRoles(RoleManager<IdentityRole> roleManager)
        {

            foreach (var role in roles)
            {
                var toDelete = await roleManager.FindByNameAsync(role);

                if (toDelete!=null)
                {
                    var delete = await roleManager.DeleteAsync(toDelete);

                    if (!delete.Succeeded)
                    {

                        throw new Exception($"Failed to delete role {role}");

                    }
                }

            }

        }
    }
}