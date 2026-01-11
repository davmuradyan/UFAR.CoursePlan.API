﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using UFAR.CoursePlan.API.Data.DAO;
using UFAR.CoursePlan.API.Data.Entities.Accounts;
using UFAR.CoursePlan.API.Data.Entities.Presons;
using UFAR.CoursePlan.API_Core.DTOs;

namespace UFAR.CoursePlan.API_Core.Services.ChairpersonSide {
    public class ChairpersonSide : IChairpersonSide {
        readonly MainDbContext context;
        public ChairpersonSide(MainDbContext context) {
            this.context = context;
        }

        public async Task<bool> CreateChairperson(ChairpersonDto chairperson) {
            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () => {
                using var transaction = await context.Database.BeginTransactionAsync();
                try {
                    // Validation
                    if (string.IsNullOrEmpty(chairperson.Name) || string.IsNullOrEmpty(chairperson.Surname) ||
                        string.IsNullOrEmpty(chairperson.Email) || string.IsNullOrEmpty(chairperson.Password)) {
                        return false;
                    }
                    // Create Chairperson Entity
                    var cp = new ChairpersonEntity {
                        Name = chairperson.Name,
                        Surname = chairperson.Surname,
                        Email = chairperson.Email,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    await context.Chairpersons.AddAsync(cp);
                    await context.SaveChangesAsync();

                    // Create Chairperson Account Entity
                    var hasher = new PasswordHasher<ChairpersonEntity>();

                    var cpAccount = new ChairpersonAccountEntity {
                        ChairpersonId = cp.Id,
                        Password = hasher.HashPassword(cp, chairperson.Password),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    await context.ChairpersonAccounts.AddAsync(cpAccount);
                    
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    
                    Console.WriteLine("[ERROR]\tChairpersonSide: Transaction failed!");
                    Console.WriteLine(ex.Message);
                    
                    return false;
                }
            });
        }
    }
}