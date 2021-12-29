<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Userdata[]|\Cake\Collection\CollectionInterface $userdata
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('New Userdata'), ['action' => 'add']) ?></li>
    </ul>
</nav>
<div class="userdata index large-9 medium-8 columns content">
    <h3><?= __('Userdata') ?></h3>
    <table cellpadding="0" cellspacing="0">
        <thead>
            <tr>
                <th scope="col"><?= $this->Paginator->sort('id') ?></th>
                <th scope="col"><?= $this->Paginator->sort('name') ?></th>
                <th scope="col"><?= $this->Paginator->sort('rank') ?></th>
                <th scope="col"><?= $this->Paginator->sort('exp') ?></th>
                <th scope="col"><?= $this->Paginator->sort('lastlogin') ?></th>
                <th scope="col"><?= $this->Paginator->sort('created') ?></th>
                <th scope="col" class="actions"><?= __('Actions') ?></th>
            </tr>
        </thead>
        <tbody>
            <?php foreach ($userdata as $userdata): ?>
            <tr>
                <td><?= $this->Number->format($userdata->id) ?></td>
                <td><?= h($userdata->name) ?></td>
                <td><?= $this->Number->format($userdata->rank) ?></td>
                <td><?= $this->Number->format($userdata->exp) ?></td>
                <td><?= h($userdata->lastlogin) ?></td>
                <td><?= h($userdata->created) ?></td>
                <td class="actions">
                    <?= $this->Html->link(__('View'), ['action' => 'view', $userdata->id]) ?>
                    <?= $this->Html->link(__('Edit'), ['action' => 'edit', $userdata->id]) ?>
                    <?= $this->Form->postLink(__('Delete'), ['action' => 'delete', $userdata->id], ['confirm' => __('Are you sure you want to delete # {0}?', $userdata->id)]) ?>
                </td>
            </tr>
            <?php endforeach; ?>
        </tbody>
    </table>
    <div class="paginator">
        <ul class="pagination">
            <?= $this->Paginator->first('<< ' . __('first')) ?>
            <?= $this->Paginator->prev('< ' . __('previous')) ?>
            <?= $this->Paginator->numbers() ?>
            <?= $this->Paginator->next(__('next') . ' >') ?>
            <?= $this->Paginator->last(__('last') . ' >>') ?>
        </ul>
        <p><?= $this->Paginator->counter(['format' => __('Page {{page}} of {{pages}}, showing {{current}} record(s) out of {{count}} total')]) ?></p>
    </div>
</div>
