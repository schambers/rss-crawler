using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RssCrawler.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] feeds = new []
			{
				"http://feeds2.feedburner.com/CodeBetter",
				"http://feeds2.feedburner.com/LosTechies",
				"http://feeds2.feedburner.com/Devlicious"
			};

			string[] words = new []
			{
				"nhibernate",
				"bdd"
			};

			System.Console.WriteLine("Beginning Processing Feeds");

			Runner.FeedRunner runner = new Runner.FeedRunner();
			runner.Crawl(feeds);

			
			System.Console.ReadLine();
		}
	}
}