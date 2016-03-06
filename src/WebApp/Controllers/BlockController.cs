﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinChainExplorerForAspNet5.Models;
using Core.Bitcoin;
using Microsoft.AspNet.Mvc;


namespace BitcoinChainExplorerForAspNet5.Controllers
{
    public class BlockController : Controller
    {
        private readonly IBitcoinBlockRepository _bitcoinBlockRepository;
        private readonly IOutputsRepository _outputsRepository;
        private readonly IInputsRepository _inputsRepository;
        private readonly ITransactionRepository _transactionRepository;

        private const int ItemsOnPage = 20;

        public BlockController(IBitcoinBlockRepository bitcoinBlockRepository, IOutputsRepository outputsRepository, IInputsRepository inputsRepository, ITransactionRepository transactionRepository)
        {
            _bitcoinBlockRepository = bitcoinBlockRepository;
            _outputsRepository = outputsRepository;
            _inputsRepository = inputsRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id, int page)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Home");

            var getBlock = await _bitcoinBlockRepository.GetAsync(id);

            if (getBlock == null)
                return View("_NotFound");

           // var outputs = await _outputsRepository.GetAsync(getBlock.Hash);
           // var inputs = await _inputsRepository.GetAsync(getBlock.Hash);
            var transactions = await _transactionRepository.GetAsync(getBlock.Hash, page, ItemsOnPage);

            var model = new BlockModel
            {
                Block = getBlock,
                Transactions = transactions,
                Count = (int) (getBlock.TotalTransactions - 1) / ItemsOnPage + 1,
                CurrentPage = page
            };



       /*     foreach (var output in outputs)
            {
                if (!model.Transactions.ContainsKey(output.Txid))
                    model.Transactions.Add(output.Txid,
                        new TransactionModel { Outputs = new List<IOutput>(), Inputs = new IInput[0], Transactions = new List<ITransaction>() });

                model.Transactions[output.Txid].Outputs.Add(output);
            }

            foreach (var transaction in transactions)
            {
                if (model.Transactions.ContainsKey(transaction.Txid))
                {
                    model.Transactions[transaction.Txid].Time = transaction.Time;
                    model.Transactions[transaction.Txid].Confirmations = transaction.Confirmations;
                }

            }*/

            return View(model);

        }
    }
}
