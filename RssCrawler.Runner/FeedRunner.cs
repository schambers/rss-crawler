using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace RssCrawler.Runner
{
	public class FeedRunner
	{
		public int ArticleCount { get; set; }
		private Mutex _mutex = new Mutex();

		public void Crawl(string[] feeds)
		{
			foreach (var feed in feeds)
			{
				var feedFetcher = new FeedFetcher(feed, this);
				Console.WriteLine("Starting Feed:{0}", feed);
				Console.ReadLine();
				new Thread(feedFetcher.ProcessFeed).Start();
			}
		}

		public void UpdateCount()
		{
			lock(_mutex)
			{
				ArticleCount++;
			}
		}
	}

	public class Article
	{
		public string Title { get; set; }
		public string Link { get; set; }
		public string Description { get; set; }
	}

	public class FeedFetcher
	{
		private FeedRunner Runner { get; set; }
		private readonly string _feedUrl;

		public FeedFetcher(string feedUrl, FeedRunner runner)
		{
			Runner = runner;
			_feedUrl = feedUrl;
		}

		public void ProcessFeed()
		{
			WebRequest request = WebRequest.Create(_feedUrl);
			using (var response = request.GetResponse())
			{
				var xmlReader = XmlReader.Create(response.GetResponseStream());
				XDocument document = XDocument.Load(xmlReader);

				IEnumerable<Article> articles = document.Descendants("channel")
					.Elements("item")
					.Select(i => new Article { Title = i.Element("title").Value, Link = i.Element("link").Value});

				foreach (var article in articles)
				{
					Runner.UpdateCount();
					Console.WriteLine("#{0}  Article:{1}", Runner.ArticleCount, article.Title);
				}
			}
		}

	}
}