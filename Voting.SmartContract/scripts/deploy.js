const hre = require("hardhat");

async function main() {
	const votingEngine = await hre.ethers.getContractFactory("VotingEngine");
 
	// Start deployment, returning a promise that resolves to a contract object
	const voting_engine = await votingEngine.deploy();
	console.log("Contract deployed to address:", voting_engine.address);
 }

// We recommend this pattern to be able to use async/await everywhere
// and properly handle errors.
main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
