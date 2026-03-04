using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void UnitTestsSimplePasses()
    {
        GameObject testObject = new GameObject("PlayerStats_Test");
        PlayerStats stats = testObject.AddComponent<PlayerStats>();

        stats.maxHealth = 100f;
        stats.currentHealth = 50f;

        stats.ModifyStat("Health", 20f);

        Assert.That(stats.maxHealth, Is.EqualTo(120f).Within(0.001f));
        Assert.That(stats.currentHealth, Is.EqualTo(60f).Within(0.001f));

        

        Object.DestroyImmediate(testObject);
    }
}